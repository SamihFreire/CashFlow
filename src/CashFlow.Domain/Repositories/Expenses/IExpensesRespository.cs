using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesRespository
    {
        public void Add(Expense expense);
    }
}
