using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _respository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
        {
            _respository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.Get();

            // usando o .Map para traduzir a request para a nossa classe de destino
            var expense = _mapper.Map<Expense>(request);
            expense.UserId = loggedUser.Id;

            await _respository.Add(expense);
            await _unitOfWork.Commit();

            // devolvendo a conversao da nossa entidade para a entidade de resposta
            return _mapper.Map<ResponseRegisteredExpenseJson>(expense);
        }

        private void Validate(RequestExpenseJson request)
        {
            // Utilizando a biblioteca do FluentValidation para realizar as validações
            // Criado a classe RegisterExpenseValidator onde foram registradas as validações que serão realizadas
            var validator = new ExpenseValidator();

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
