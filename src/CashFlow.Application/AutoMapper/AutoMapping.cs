using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper
{
    // Precisa herdar da classe Profile que vem do AutoMapper
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestExpenseJson, Expense>();
            CreateMap<RequestRegisterUserJson, User>();
        }

        private void EntityToResponse()
        {
            CreateMap<Expense,ResponseRegisteredExpenseJson>();
            CreateMap<Expense,ResponseShortExpenseJson>();
            CreateMap<Expense, ResponseExpenseJson>();
        }
    }
}
