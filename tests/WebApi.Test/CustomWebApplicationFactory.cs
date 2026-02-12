using CashFlow.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    // CustomWebApplicationFactory é uma classe personalizada que herda de WebApplicationFactory<Program> (disponibiliza um servidor de teste), onde Program é a classe principal da aplicação web. Essa classe é usada para configurar o ambiente de teste e criar um servidor de teste para a aplicação web.
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
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
                });
        }
    }
}
