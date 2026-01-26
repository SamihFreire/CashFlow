using CashFlow.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.Delete
{
    public interface IDeleteExpenseUseCase
    {
        public Task Execute(long id);
    }
}
