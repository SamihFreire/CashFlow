using CashFlow.Api.Filters;
using CashFlow.Api.Middleware;
using CashFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurando para que o Filtro de exception seja utilizado
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Configurando injeção de dependencia
// Criado um método de extensão onde a classe e o metodo da classe sao static
// ja deixa explicito que a função recebe como parametro o valor de quem ta chamando que no caso é o builder.Services
builder.Services.AddInfrastructure();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
