using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ExpensesReadOnlyRepositoryBuilder
    {
        private readonly Mock<IExpensesReadOnlyRepository> _repository;

        public ExpensesReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpensesReadOnlyRepository>();
        }

        public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
        {
            _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

            return this;
        }

        public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
        {
            if (expense is not null)
                _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);

            return this;
        }

        public ExpensesReadOnlyRepositoryBuilder FilterByMonth(User user, List<Expense> expenses)
        {
            // It.IsAny<DateOnly>() falando para o mock aceitar qualquer valor do tipo DateOnly,
            // já que o método FilterByMonth tem um parâmetro do tipo DateOnly e não estamos interessados em testar a lógica de filtragem por mês aqui,
            // mas sim em garantir que o método retorne a lista de despesas correta para o usuário fornecido.
            _repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);

            return this;
        }

        public IExpensesReadOnlyRepository Build() => _repository.Object;
    }
}
