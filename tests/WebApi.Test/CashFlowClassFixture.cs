using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test
{
    // IClassFixture é utilizado para compartilhar a instância do WebApplicationFactory entre os testes
    // WebApplicationFactory cria um servidor de teste para a aplicação web
    // Program é a classe parcial criada no projeto principal para permitir testes de integração
    public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient; // HttpClient é utilizado para enviar requisições HTTP para o servidor de teste

        public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient(); // Criando uma instância do HttpClient a partir do WebApplicationFactory, que será usada para enviar requisições para o servidor de teste
        }

        protected async Task<HttpResponseMessage> DoPost(
            string requestUri,
            object request,
            string token = "",
            string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(requestUri, request);
        }

        protected async Task<HttpResponseMessage> DoGet(
        string requestUri,
        string token,
        string culture = "en")
        {
            AuthorizeRequest(token);
            ChangeRequestCulture(culture);

            return await _httpClient.GetAsync(requestUri);
        }

        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        private void ChangeRequestCulture(string culture)
        {
            // Limpando os headers de Accept-Language para garantir que apenas a cultura especificada seja utilizada na requisição
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        }
    }
}
