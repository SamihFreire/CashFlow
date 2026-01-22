using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.GetById
{
    public class GetExpenseByUdUseCase : IGetExpenseByUdUseCase
    {
        private readonly IExpensesRespository _respository;
        private readonly IMapper _mapper;
        public GetExpenseByUdUseCase(IExpensesRespository respository, IMapper mapper)
        {
            _respository = respository;
            _mapper = mapper;
        }
        public async Task<ResponseExpenseJson> Execute(long id)
        {
            var result = await _respository.GetById(id);

            if(result is null)
            {
                throw new NotFoundExcpetion(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
