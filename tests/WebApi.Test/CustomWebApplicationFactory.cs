using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    // CustomWebApplicationFactory é uma classe personalizada que herda de WebApplicationFactory<Program> (disponibiliza um servidor de teste), onde Program é a classe principal da aplicação web. Essa classe é usada para configurar o ambiente de teste e criar um servidor de teste para a aplicação web.
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Expense { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Admin { get; private set; } = default!;

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

                    var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    // Iniciando o banco de dados com dados de teste
                    StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
                    
                });
        }

        // Sempre que um teste for executado, o método StartDataBase será chamado para garantir que o banco de dados em memória seja inicializado com os dados necessários para os testes.
        // Isso é especialmente útil para garantir que os testes sejam consistentes e independentes, já que cada teste pode começar com um estado conhecido do banco de dados.
        private void StartDatabase(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = AddUsersTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
            AddExpenses(dbContext, user);

            dbContext.SaveChanges();
        }

        private User AddUsersTeamMember(
            CashFlowDbContext dbContext, 
            IPasswordEncripter passwordEncripter, 
            IAccessTokenGenerator accessTokenGenerator)
        {
            // Criando um usuário de teste usando o UserBuilder e adicionando-o ao banco de dados em memória
            var user = UserBuilder.Build();// Armazenando a senha original para uso nos testes
            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password); // Encriptando a senha do usuário antes de adicioná-lo ao banco de dados

            dbContext.Users.Add(user);

            var token = accessTokenGenerator.Generate(user); // Gerando um token de acesso para o usuário de teste criado no banco de dados em memória

            // Criando uma instância de UserIdentityManager para o usuário de teste, que pode ser usada nos testes para obter informações sobre o usuário, como nome, email, senha e token de acesso
            User_Team_Member = new UserIdentityManager(user, password, token);

            return user;
        }

        private void AddExpenses(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);

            dbContext.Expenses.Add(expense);

            Expense = new ExpenseIdentityManager(expense);
        }
    }
}
