using System.Globalization;

namespace CashFlow.Api.Middleware
{
    public class CultureMiddleware
    {
        public readonly RequestDelegate _next;

        // O RequestDelegate é um interceptador, sendo o responsavel por permitir ou não o fluxo de seguir para o endpoint
        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // É preciso criar essa função com esse exato nome
        // O context é usado para podermos acessar a requisição recebida
        public async Task Invoke(HttpContext context)
        {
            // Buscando todas as linguagens suportadas pelo .net
            var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

            // Extraindo do Header da requisição a linguagem que aplicação deseja
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

            // Definindo que a linguagem padrão é Inglês
            var cultureInfo = new CultureInfo("en");
            
            // Caso seja especificado a linguagem no header
            // E a linguagem solicitada é suportada pelo .net
            // ele define a linguagem solicitada
            if (!string.IsNullOrWhiteSpace(requestedCulture) 
                && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
            {
                // Passando como parametro a linguagem solicitada
                cultureInfo = new CultureInfo(requestedCulture);
            }

            // Altera a API para devolver a cultura correta
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            // Permitindo o fluxo continuar
            // Segue para o endpoint
            await _next(context);
        }
    }
}
