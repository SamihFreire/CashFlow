using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        // O método IsTestEnvironment é uma extensão para a interface IConfiguration, que é usada para acessar as configurações da aplicação.
        // Ele verifica se a configuração "InMemoryTests" está definida como true, o que indica que estamos em um ambiente de teste com banco de dados em memória.
        // Isso é útil para diferenciar o comportamento da aplicação entre ambientes de desenvolvimento, produção e teste, permitindo que os testes sejam executados em um ambiente controlado e isolado.
        public static bool IsTestEnvironment(this IConfiguration configuration)
        {
            // Busca a configuração "InMemoryTests" e retorna seu valor como booleano. Se a configuração não estiver definida, o valor padrão será false.
            // Essa configuração pode ser definida em um arquivo de configuração específico para testes, como appsettings.Test.json, ou em variáveis de ambiente durante a execução dos testes.
            return configuration.GetValue<bool>("InMemoryTest"); 
        }
    }
}
