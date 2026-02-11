using CashFlow.Application.UseCase.Users;
using CashFlow.Communication.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory] // Utilizando o Theory para testar múltiplos casos de teste
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")] // testa com 8 caracters que deveria passar, porem nao possui letras maiusculas
        [InlineData("Aaaaaaaa")] // testa com 8 caracters e letra maiuscula que deveria passar, porem nao possui numeros
        [InlineData("Aaaaaaa1")] // testa com 8 caracters, letra maiuscula e numero que deveria passar, porem nao possui caracteres especiais
        public void Error_Password_Invalid(string password)
        {
            var validator = new PasswordValidator<RequestRegisterUserJson>(); // Criando uma instância do PasswordValidator para o tipo RequestRegisterUserJson

            var result = validator
                .IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password); // Validando a senha utilizando o método IsValid do PasswordValidator e passando um contexto de validação vazio e a senha a ser testada

            result.Should().BeFalse(); // Verificando se o resultado é inválido
        }
    }
}
