using CashFlow.Application.UseCase.Expenses.Delete;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Tokens;
using CashFlow.Infrastructure.Services.LoggedUser;
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
            services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
            services.AddScoped<ILoggedUser, LoggedUser>();
            
            AddToken(services, configuration);
            AddRespositories(services);

            // Verifica se está em ambiente de teste
            if (configuration.IsTestEnvironment() == false)
            {
                // Não está em ambiente de teste, então configura o DbContext normalmente
                AddDbContext(services, configuration);
            }

        }

        private static void AddToken(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
            var signinKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signinKey!));
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
            
            var serverVersion = ServerVersion.AutoDetect(conncetionString);

            //optionsBuilder.UseMySql(conncetionString, serverVersion);

            services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(conncetionString, serverVersion));
        }
    }
}
