using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.GetAll
{
    public class GetAllExpenseUseCase : IGetAllExpenseUseCase
    {
        private readonly IExpensesReadOnlyRepository _respository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetAllExpenseUseCase(IExpensesReadOnlyRepository respository, IMapper mapper, ILoggedUser loggedUser)
        {
            _respository = respository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseExpensesJson> Execute()
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _respository.GetAll(loggedUser);

            return new ResponseExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }
    }
}
