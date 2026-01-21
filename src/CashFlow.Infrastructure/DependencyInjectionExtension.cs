using CashFlow.Domain.Repositories.Expenses;
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
            services.AddScoped<IExpensesRespository, ExpensesRepository>();
        }
    }
}
