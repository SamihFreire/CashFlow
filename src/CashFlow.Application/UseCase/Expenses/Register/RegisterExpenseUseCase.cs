using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesRespository _respository;

        public RegisterExpenseUseCase(IExpensesRespository repository)
        {
            _respository = repository;
        }
        public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            var entity = new Expense
            {
                Amount = request.Amount,
                Date = request.Date,
                Description = request.Description,
                Title = request.Title,
                PaymentType = (PaymentType)request.PaymentType,
            };

            _respository.Add(entity);

            return new ResponseRegisteredExpenseJson();
        }

        private void Validate(RequestRegisterExpenseJson request)
        {
            // Utilizando a biblioteca do FluentValidation para realizar as validações
            // Criado a classe RegisterExpenseValidator onde foram registradas as validações que serão realizadas
            var validator = new RegisterExpenseValidator();

            // Função da biblioteca que realiza as validações criadas
            var result = validator.Validate(request);

            // Caso possua erros de validação
            // Obtem a lista de erros do FluentValidation e lança a exceção para tratamento
            // ErrorOnValidationException recebe a lista de erros e todo erro cai na ExceptionFilter para tratamento onde estao os erros mapeados
            if (!result.IsValid)
            {
                var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
