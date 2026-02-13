using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser
{
    internal class LoggedUser : ILoggedUser
    {
        private readonly CashFlowDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(CashFlowDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> Get()
        {
            string token = _tokenProvider.TokenOnRequest();

            // inicializa uma instância de JwtSecurityTokenHandler para ler o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            // lê o token JWT e extrai as reivindicações (claims) contidas nele
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            // procura a reivindicação (claim) do tipo ClaimTypes.Sid e obtém seu valor, que é o identificador do usuário
            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            // consulta o banco de dados para encontrar o usuário correspondente ao identificador extraído do token JWT
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
        }
    }
}
