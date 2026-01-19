using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;

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
            // Verificando se o result é verdadeiro com a classe Assert
            Assert.True(result.IsValid);
        }
    }
}
