using CashFlow.Domain.Security.Cryptography;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncrypterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();
            _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("!%djsbndisbn342");
        }

        public PasswordEncrypterBuilder Verify(string? password)
        {
            if(string.IsNullOrWhiteSpace(password) == false)
                _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);

            return this;
        }

        public IPasswordEncripter Build() => _mock.Object;
    }
}
