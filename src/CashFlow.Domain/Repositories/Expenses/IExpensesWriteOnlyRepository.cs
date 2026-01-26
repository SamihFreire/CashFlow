using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        public Task Add(Expense expense);

        /// <summary>
        /// A função retorna TRUE se o processo de remoção foi um sucesso caso contrario FALSE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> Delete(long id);
    }
}
