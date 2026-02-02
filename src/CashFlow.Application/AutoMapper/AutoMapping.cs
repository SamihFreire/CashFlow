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
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(user => user.Password, config => config.Ignore()); // Mapea todas as propriedades e ignora o password
        }

        private void EntityToResponse()
        {
            CreateMap<Expense,ResponseRegisteredExpenseJson>();
            CreateMap<Expense,ResponseShortExpenseJson>();
            CreateMap<Expense, ResponseExpenseJson>();
        }
    }
}
