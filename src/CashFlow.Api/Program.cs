using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Application;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Configurando authorization no swagger
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey,
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
#endregion

builder.Services.AddSwaggerGen(options => {
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
});

// Configurando para que o Filtro de exception seja utilizado
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Configurando injeção de dependencia
// Criado um método de extensão onde a classe e o metodo da classe sao static
// ja deixa explicito que a função recebe como parametro o valor de quem ta chamando que no caso é o builder.Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

#region indicando pra API que vai ser usado o authentication

var signinKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey");

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey!))
    };
});

#endregion

builder.Services.AddRouting(option => option.LowercaseUrls = true); // Forca todas as urls serem minusculas

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurando o Middleware
// Precisa ser chamado depois do -> var app = builder.Build();
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); // Configurando o authentication
app.UseAuthorization();

app.MapControllers();

// Verificando se está em ambiente de teste
// Utilizando o método de extensão IsTestEnvironment criado na Infrastructure
// Ele verifica se a configuração "InMemoryTests" está definida como true
// Se estiver, significa que estamos em um ambiente de teste com banco de dados em memória
if(builder.Configuration.IsTestEnvironment() == false)
{
    // Executando as migrations apenas se não estiver em ambiente de teste
    await MigrateDatabase(); // Executando de forma automatica as migrations
}

app.Run();

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();

    await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}

// Classe parcial para permitir testes de integração
public partial class  Program { }