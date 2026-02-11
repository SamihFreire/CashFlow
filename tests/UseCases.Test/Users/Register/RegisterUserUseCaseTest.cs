using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Domain.Repositories.User;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.Should().NotBeNull(); // Verifica se o resultado não é nulo (Não deve ser null)
            result.Name.Should().Be(request.Name); // Verifica se o nome retornado é igual ao nome do request
            result.Token.Should().NotBeNullOrWhiteSpace(); // Verifica se o token retornado não é nulo ou vazio (Deve conter um token válido)
        }

        private RegisterUserUseCase CreateUseCase() // Método auxiliar para criar uma instância do RegisterUserUseCase com dependências simuladas
        {
            var mapper = MapperBuilder.Build(); // Cria um mapper usando o MapperBuilder, que é uma classe auxiliar para configurar o AutoMapper para os testes
            var unitOfWork = UnitOfWorkBuilder.Build(); // Cria um unit of work usando o UnitOfWorkBuilder, que é uma classe auxiliar para criar uma implementação fake da interface IUnitOfWork usando a biblioteca Moq
            var writeRepository = UserWriteOnlyRepositoryBuilder.Build(); // Cria um repositório de escrita para usuários usando o UserWriteOnlyRepositoryBuilder, que é uma classe auxiliar para criar uma implementação fake da interface IUserWriteOnlyRepository usando a biblioteca Moq
            var passwordEncripter = PasswordEncripterBuilder.Build(); // Cria um encriptador de senhas usando o PasswordEncripterBuilder, que é uma classe auxiliar para criar uma implementação fake da interface IPasswordEncripter usando a biblioteca Moq
            var tokenGenerator = JwtTokenGeneratorBuilder.Build(); // Cria um gerador de tokens JWT usando o JwtTokenGeneratorBuilder, que é uma classe auxiliar para criar uma implementação fake da interface IAccessTokenGenerator usando a biblioteca Moq
            var readRepository = new UserReadOnlyRepositoryBuider().Build(); // Cria um repositório de leitura para usuários usando o UserReadOnlyRepositoryBuider, que é uma classe auxiliar para criar uma implementação fake da interface IUserReadOnlyRepository usando a biblioteca Moq

            return new RegisterUserUseCase(mapper, passwordEncripter, readRepository, writeRepository, unitOfWork, tokenGenerator);
        }
    }
}
