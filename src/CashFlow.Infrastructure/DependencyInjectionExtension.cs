using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    // Criando um método de extensao
    // Definimos a classe e a função como static, e no parametro usamos o this
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            AddDbContext(services);
            AddRespositories(services);
        }

        // Utilizada para configurar a injeção de dependendia dos repositories
        private static void AddRespositories(IServiceCollection services)
        {
            services.AddScoped<IExpensesRespository, ExpensesRepository>();
        }

        private static void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<CashFlowDbContext>();
        }
    }
}
