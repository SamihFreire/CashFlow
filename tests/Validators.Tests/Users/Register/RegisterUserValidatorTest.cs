using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.Register
{
    public class RegisterUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            // Arrangen
            var validator = new RegisterUserValidator(); // Instanciando o validador
            var request = RequestRegisterUserJsonBuilder.Build(); // Busacando os dados de request para o teste

            // Act
            var result = validator.Validate(request); // Validando o request

            // Assert
            result.IsValid.Should().BeTrue(); // Verificando se o resultado é válido
        }

        [Theory] // Utilizando o Theory para testar múltiplos casos de teste
        [InlineData("")] 
        [InlineData("      ")]
        [InlineData(null)]
        public void Error_Name_Empty(string name)
        {
            // Arrangen
            var validator = new RegisterUserValidator(); // Instanciando o validador
            var request = RequestRegisterUserJsonBuilder.Build(); // Busacando os dados de request para o teste
            request.Name = name; // Atribuindo o valor de name que será testado para o request

            // Act
            var result = validator.Validate(request); // Validando o request

            // Assert
            result.IsValid.Should().BeFalse(); // Verificando se o resultado é inválido
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY)); // Verificando se a mensagem de erro é a esperada
        }

        [Theory] // Utilizando o Theory para testar múltiplos casos de teste
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Error_Email_Empty(string email)
        {
            // Arrangen
            var validator = new RegisterUserValidator(); // Instanciando o validador
            var request = RequestRegisterUserJsonBuilder.Build(); // Busacando os dados de request para o teste
            request.Email = email; // Atribuindo o valor de email que será testado para o request

            // Act
            var result = validator.Validate(request); // Validando o request

            // Assert
            result.IsValid.Should().BeFalse(); // Verificando se o resultado é inválido
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY)); // Verificando se a mensagem de erro é a esperada
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            // Arrangen
            var validator = new RegisterUserValidator(); // Instanciando o validador
            var request = RequestRegisterUserJsonBuilder.Build(); // Busacando os dados de request para o teste
            request.Email = "teste.com"; 

            // Act
            var result = validator.Validate(request); // Validando o request

            // Assert
            result.IsValid.Should().BeFalse(); // Verificando se o resultado é inválido
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID)); // Verificando se a mensagem de erro é a esperada
        }

        [Fact]
        public void Error_Password_Empty()
        {
            // Arrangen
            var validator = new RegisterUserValidator(); // Instanciando o validador
            var request = RequestRegisterUserJsonBuilder.Build(); // Busacando os dados de request para o teste
            request.Password = string.Empty;

            // Act
            var result = validator.Validate(request); // Validando o request

            // Assert
            result.IsValid.Should().BeFalse(); // Verificando se o resultado é inválido
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD)); // Verificando se a mensagem de erro é a esperada
        }
    }
}
