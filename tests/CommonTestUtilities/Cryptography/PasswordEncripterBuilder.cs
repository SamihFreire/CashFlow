using CashFlow.Domain.Security.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPasswordEncripter Build()
        {
            var mock = new Mock<IPasswordEncripter>();

            // Sinalizando ao mock que quando chamar o encrypt com qualquer string, ele deve retornar a string "!%djsbndisbn342".
            // Isso é útil para simular o comportamento do método Encrypt sem realmente realizar a criptografia, permitindo que os testes sejam executados de forma consistente e previsível.
            mock.Setup(passwordEcncript => passwordEcncript.Encrypt(It.IsAny<string>())).Returns("!%djsbndisbn342");

            return mock.Object;
        }
    }
}
