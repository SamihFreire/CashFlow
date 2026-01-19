using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenserValidatorTests
    {
        // Com o Fact sinalizamos que se trata de uma função de teste de unidade
        [Fact]
        public void Success()
        {
            // Arrange: Criação das instancias de tudo que precisamos para executar os testes
            
            // Precisamos adicionar a referencia de application para ter acesso ao RegisterExpenseValidator
            var validator = new RegisterExpenseValidator();

            // Utilizando a classe de build onde obtemos dados fakes com o pacote Bogus
            var request = RequestRegisterExpenseJsonBuilder.Build();

            // Act: Ação que queremos testa, no caso testar vamos testar se esta válido os dados da requisição
            var result = validator.Validate(request);

            // Assert: Compara o resultado obtido com o resultado esperado
            // Verificando se o result é verdadeiro com o pacote FluentAssertions
            result.IsValid.Should().BeTrue(); // Ela 'DEVERIA' ser true
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData(null)]
        public void Error_Title_Empty(string title)
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Title = title;

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse(); // A validação deve ser falsa

            // Deve conter apenas um erro e o erro deve ser que o titúlo é obrigatório
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED)); 
        }

        [Fact]
        public void Error_Date_Future()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Date = DateTime.Now.AddDays(1); // Forçando a data ser no futuro

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse(); // A validação deve ser falsa

            // Deve conter apenas um erro e o erro deve ser que a data nao pode ser no futuro
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE));
        }

        [Fact]
        public void Error_Payment_Type_Invalid()
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.PaymentType = (PaymentType)700; // Forçando o tipo de pagamento ser invalido

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse(); // A validação deve ser falsa

            // Deve conter apenas um erro e o erro deve ser que o tipo de pagamento é inválido
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
        }

        [Theory] // Permite nesse caso definir os parametros que a função vai receber na variavel amount
        [InlineData(0)] //Cada InlineData desse é um parametro que será testado
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-7)]
        public void Error_Amount_Invalid(decimal amount)
        {
            // Arrange
            var validator = new RegisterExpenseValidator();
            var request = RequestRegisterExpenseJsonBuilder.Build();
            request.Amount = amount; // Forçando o valor ser negativo

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse(); // A validação deve ser falsa

            // Deve conter apenas um erro e o erro deve ser maior que zero
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
        }
    }
}
