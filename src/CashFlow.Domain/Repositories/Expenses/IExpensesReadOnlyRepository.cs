using CashFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Domain.Repositories.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        public Task<List<Expense>> GetAll();
        public Task<Expense?> GetById(long id);
    }
}
