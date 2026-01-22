using CashFlow.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.GetAll
{
    public interface IGetAllExpenseUseCase
    {
        public Task<ResponseExpensesJson> Execute();
    }
}
