using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    // Classe onde fica as regras de validação do RequestRegisterExpenseJson
    public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
    {
        public RegisterExpenseValidator()
        {
            RuleFor(expense => expense.Title).NotEmpty().WithMessage("The title is required.");
            RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("The Amount must be grater tha zero."); // Valore Maiores que zero
            RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be for the future."); // Data menor ou igual
            RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("Payment type is not valid."); // Verifica se é um Enum
        }
    }
}
