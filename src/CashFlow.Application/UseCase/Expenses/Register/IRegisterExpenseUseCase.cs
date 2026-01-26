using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        public Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request);
    }
}
