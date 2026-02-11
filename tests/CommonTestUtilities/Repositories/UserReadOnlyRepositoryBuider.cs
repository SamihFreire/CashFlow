using CashFlow.Domain.Repositories.User;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuider
    {
        private readonly Mock<IUserReadOnlyRepository> _repository; // Criando um campo privado do tipo Mock<IUserReadOnlyRepository> para armazenar a instância do mock do repositório de leitura de usuários.

        public UserReadOnlyRepositoryBuider() // Criando um construtor público para a classe UserReadOnlyRepositoryBuider, onde a instância do mock do repositório de leitura de usuários é inicializada.
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }
        public void ExistActiveUserWithEmail(string email) // Criando um método público chamado ExistActiveUserWithEmail que recebe um parâmetro do tipo string representando o email. Esse método é utilizado para configurar o comportamento do mock do repositório de leitura de usuários para simular a existência de um usuário ativo com o email fornecido.
        {
            _repository.Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
        }

        public IUserReadOnlyRepository Build() => _repository.Object; // Criando um método público chamado Build que retorna uma instância do tipo IUserReadOnlyRepository. O método retorna a implementação fake do repositório de leitura de usuários, que é obtida através da propriedade Object do mock.
    
    }
}
