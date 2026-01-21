using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesRespository
    {
        public void add(Expense expense);
    }
}
