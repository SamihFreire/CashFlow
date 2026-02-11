using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build()
        {
            var mock = new Mock<IUnitOfWork>(); // Cria um mock da interface IUnitOfWork usando a biblioteca Moq. O mock é uma implementação fake que pode ser configurada para simular o comportamento da interface.

            return mock.Object; // Retorna uma implementação fake dessa interface, onde os métodos não fazem nada, mas permitem que o teste seja executado sem erros de dependência.
        }
    }
}
