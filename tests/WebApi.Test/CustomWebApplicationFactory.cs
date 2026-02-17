using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
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
        private string _token;

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
                    StartDatabase(dbContext, passwordEncripter);
                    
                    // Obtendo uma instância do IAccessTokenGenerator para gerar tokens de acesso
                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    _token = tokenGenerator.Generate(_user); // Gerando um token de acesso para o usuário de teste criado no banco de dados em memória
                });
        }

        public string GetName() => _user.Name;
        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public string GetToken() => _token;

        

        // Sempre que um teste for executado, o método StartDataBase será chamado para garantir que o banco de dados em memória seja inicializado com os dados necessários para os testes.
        // Isso é especialmente útil para garantir que os testes sejam consistentes e independentes, já que cada teste pode começar com um estado conhecido do banco de dados.
        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            AddUsers(dbContext, passwordEncripter);
            AddExpenses(dbContext, _user);

            dbContext.SaveChanges();
        }

        private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
        {
            // Criando um usuário de teste usando o UserBuilder e adicionando-o ao banco de dados em memória
            _user = UserBuilder.Build();// Armazenando a senha original para uso nos testes
            _password = _user.Password;

            _user.Password = passwordEncripter.Encrypt(_user.Password); // Encriptando a senha do usuário antes de adicioná-lo ao banco de dados

            dbContext.Users.Add(_user);
        }

        private void AddExpenses(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);

            dbContext.Expenses.Add(expense);
        }
    }
}
