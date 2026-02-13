using CashFlow.Infrastructure.Security.Tokens;

namespace CashFlow.Api.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        // Permite acessar os dados da requisição HTTP atual
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public string TokenOnRequest()
        {
            // Acessa o cabeçalho de autorização da requisição HTTP atual e converte para string
            // Tendo acesso ao Token que está sendo enviado na requisição
            var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

            // Remove a parte "Bearer " do início do token e retorna apenas o valor do token em si, sem o prefixo "Bearer "
            // o .Length.. é utilizado para pegar a substring a partir do índice especificado, ou seja, a partir do comprimento da string "Bearer "
            return authorization["Bearer ".Length..].Trim();
        }
    }
}
