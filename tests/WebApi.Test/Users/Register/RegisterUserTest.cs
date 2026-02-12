using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Users.Register
{
    // Teste de integração para o endpoint de registro de usuário
    // IClassFixture é utilizado para compartilhar a instância do WebApplicationFactory entre os testes
    // WebApplicationFactory cria um servidor de teste para a aplicação web
    // Program é a classe parcial criada no projeto principal para permitir testes de integração
    public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient; // HttpClient é utilizado para enviar requisições HTTP para o servidor de teste
        private readonly string METHOD = "api/user";

        public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient(); // Criando uma instância do HttpClient a partir do WebApplicationFactory, que será usada para enviar requisições para o servidor de teste
        }

        [Fact]
        public async Task Success()
        {
            // Criando um objeto de requisição para o registro de usuário utilizando o RequestRegisterUserJsonBuilder, que é uma classe auxiliar para construir objetos de requisição com dados de teste
            var request = RequestRegisterUserJsonBuilder.Build();

            // Enviando uma requisição POST para o endpoint de registro de usuário, passando o objeto request como JSON no corpo da requisição
            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            // Verificando se o status code da resposta é Created (201), indicando que o usuário foi registrado com sucesso
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            // Lendo o conteúdo da resposta como um stream, que pode ser utilizado para verificar os dados retornados pelo endpoint, se necessário
            var body = await result.Content.ReadAsStreamAsync();

            // Parseando o conteúdo da resposta como um JsonDocument para facilitar a leitura dos dados retornados pelo endpoint
            var response = await JsonDocument.ParseAsync(body);

            // Verificando se o nome do usuário retornado na resposta é igual ao nome enviado na requisição, garantindo que os dados foram processados corretamente pelo endpoint
            response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
            response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();

        }
    }
}
