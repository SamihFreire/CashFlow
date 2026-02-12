using System.Collections;

namespace WebApi.Test.InlineData
{
    // CultureInlineDataTest é uma classe que implementa a interface IEnumerable<object[]>, permitindo que seja utilizada como fonte de dados para testes parametrizados em xUnit.
    // Cada objeto[] representa um conjunto de dados que será passado para o teste, nesse caso, cada string representa um código de cultura (como "en" para inglês, "fr" para francês, etc.) que pode ser utilizado para testar a internacionalização da aplicação.
    public class CultureInlineDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // o yield return é utilizado para retornar um valor a cada iteração do enumerador, permitindo que os testes sejam executados de forma iterativa para cada conjunto de dados fornecido.
            // Cada yield return retorna um array de objetos contendo um código de cultura diferente, permitindo que os testes sejam executados para cada cultura especificada.
            yield return new object[] { "en" };
            yield return new object[] { "fr" };
            yield return new object[] { "pt-BR" };
            yield return new object[] { "pt-PT" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
