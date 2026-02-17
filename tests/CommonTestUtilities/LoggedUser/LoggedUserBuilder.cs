using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();

            //  O returnAsync com o user passado como parâmetro serve para configurar o comportamento do método Get() do ILoggedUser.
            // Quando o método Get() for chamado durante os testes, ele retornará o objeto user que foi passado para o método Build.
            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
