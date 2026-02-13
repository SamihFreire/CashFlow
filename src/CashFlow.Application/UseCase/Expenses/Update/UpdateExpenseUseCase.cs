using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCase.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpensesUpdateOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        public UpdateExpenseUseCase(IMapper mapper, IUnitOfWork unitOfWork, IExpensesUpdateOnlyRepository repository, ILoggedUser loggedUser)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, RequestExpenseJson request)
        {
            Validate(request);

            // Buscando o usuário logado para verificar a quem pertence a despesa
            var loggedUser = await _loggedUser.Get();

            // Buscando a despesa para verificar se ela existe e se pertence ao usuário logado
            var expense = await _repository.GetById(loggedUser, id);
            
            if (expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            _mapper.Map(request, expense); // Ele vai pegar os dados que estao em request e levar para o objeto ja criado expense
            //_mapper.Map<Expense>(request); //Ele vai instanciar um novo objeto do tipo Expense levando todos os dados que estão em request

            _repository.Update(expense);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(request);

            if(!result.IsValid)
            {
                var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
