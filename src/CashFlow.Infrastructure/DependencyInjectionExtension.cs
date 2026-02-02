using CashFlow.Application.UseCase.Expenses.Delete;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure
{
    // Criando um método de extensao
    // Definimos a classe e a função como static, e no parametro usamos o this
    public static class DependencyInjectionExtension
    {
        // Recebendo o configuration para ter acesso connectionString
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRespositories(services);

            services.AddScoped<IPasswordEncripter, Security.BCrypt>();
        }

        // Utilizada para configurar a injeção de dependendia dos repositories
        private static void AddRespositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
            services.AddScoped<IExpensesUpdateOnlyRepository, ExpensesRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var conncetionString = configuration.GetConnectionString("Connection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 45));

            //optionsBuilder.UseMySql(conncetionString, serverVersion);
            
            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(conncetionString, serverVersion));
        }
    }
}
