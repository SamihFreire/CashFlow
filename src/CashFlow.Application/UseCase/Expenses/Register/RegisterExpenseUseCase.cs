using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            return new ResponseRegisteredExpenseJson();
        }

        private void Validate(RequestRegisterExpenseJson request)
        {
            var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title);
            if(titleIsEmpty)
                throw new ArgumentException("The title is required.");

            if(request.Amount <= 0) 
                throw new ArgumentException("The Amount must be grater tha zero.");

            var result = DateTime.Compare(request.Date, DateTime.UtcNow);
            if (result > 0)
                throw new ArgumentException("Expenses cannot be for the future.");

            var paymentTypeIsValid = Enum.IsDefined(typeof(PaymentType), request.PaymentType);
            if (!paymentTypeIsValid)
                throw new ArgumentException("Payment type is not valid.");
        }
    }
}
