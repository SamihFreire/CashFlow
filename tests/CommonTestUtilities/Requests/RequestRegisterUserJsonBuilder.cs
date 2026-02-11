using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserJsonBuilder
    {
        public static RequestRegisterUserJson Build() // Método estatico para facilitar a criação de objetos RequestRegisterUserJson em nossos testes, utilizando o Faker para gerar dados realistas
        {
            return new Faker<RequestRegisterUserJson>()
                .RuleFor(user => user.Name, faker => faker.Person.FirstName) // Geramos um nome de usuário usando o faker
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name)) // Geramos o email usando o nome do usuário para garantir que seja um email válido
                .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1")); // Informamos pelo prefix que o passdword deve conter pelo menos um caractere especial, uma letra maiúscula e um número
        }
    }
}
