using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

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

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))] // Utilizando ClassData para fornecer diferentes culturas para o teste, permitindo verificar se as mensagens de erro são retornadas no idioma correto
        public async Task Error_Empty_Name(string cultureInfo)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            // Adicionando o header Accept-Language para a requisição, indicando que a resposta deve ser retornada na linguagem que vem do parametro cultureInfo.
            // Isso é importante para garantir que as mensagens de erro sejam retornadas no idioma correto, especialmente em casos de validação de dados, onde as mensagens de erro podem variar dependendo do idioma configurado na aplicação.
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();

            var response = await JsonDocument.ParseAsync(body);

            var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

            // Obtendo a mensagem de erro esperada para o caso de nome vazio, utilizando o ResourceManager para acessar as mensagens de erro localizadas, e passando o cultureInfo para garantir que a mensagem seja retornada no idioma correto.
            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}