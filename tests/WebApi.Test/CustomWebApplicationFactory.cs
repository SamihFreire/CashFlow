using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    // CustomWebApplicationFactory é uma classe personalizada que herda de WebApplicationFactory<Program> (disponibiliza um servidor de teste), onde Program é a classe principal da aplicação web. Essa classe é usada para configurar o ambiente de teste e criar um servidor de teste para a aplicação web.
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private CashFlow.Domain.Entities.User _user;
        private string _password;

        // ConfigureWebHost é um método que pode ser sobrescrito para configurar o ambiente de teste.
        // Nesse caso, estamos definindo o ambiente como "Test", o que pode ser usado para carregar configurações específicas para testes, como bancos de dados em memória ou outras dependências de teste.
        // Isso permite que os testes sejam executados em um ambiente controlado, isolado do ambiente de produção.
        // A nossa deve conter um arquivo chamado appsettings.Test.json, onde podemos colocar as configurações específicas para o ambiente de teste, como a string de conexão para um banco de dados em memória.
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    // Criando um provedor de serviços para o banco de dados em memória
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    // Removendo a configuração do banco de dados real para usar um banco de dados em memória durante os testes
                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting"); // Usando um banco de dados em memória para os testes
                        config.UseInternalServiceProvider(provider); // Configurando o provedor de serviços para o banco de dados em memória
                    });

                    // Criando um escopo de serviço para obter uma instância do CashFlowDbContext e iniciar o banco de dados com dados de teste
                    var scope = services.BuildServiceProvider().CreateScope();

                    // Obtendo uma instância do CashFlowDbContext para iniciar o banco de dados com dados de teste
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();

                    // Obtendo uma instância do IPasswordEncripter para encriptar senhas
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

                    // Iniciando o banco de dados com dados de teste
                    StartDataBase(dbContext, passwordEncripter);
                });
        }

        public string GetName() => _user.Name;
        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;

        // Sempre que um teste for executado, o método StartDataBase será chamado para garantir que o banco de dados em memória seja inicializado com os dados necessários para os testes.
        // Isso é especialmente útil para garantir que os testes sejam consistentes e independentes, já que cada teste pode começar com um estado conhecido do banco de dados.
        private void StartDataBase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            // Criando um usuário de teste usando o UserBuilder e adicionando-o ao banco de dados em memória
            _user = UserBuilder.Build();
            _password = _user.Password; // Armazenando a senha original para uso nos testes
            _user.Password = passwordEncripter.Encrypt(_user.Password); // Encriptando a senha do usuário antes de adicioná-lo ao banco de dados

            dbContext.Users.Add(_user);

            dbContext.SaveChanges();
        }
    }
}
