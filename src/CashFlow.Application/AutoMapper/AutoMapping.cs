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
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(user => user.Password, config => config.Ignore()); // Mapea todas as propriedades e ignora o password

            CreateMap<RequestExpenseJson, Expense>()
                .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct())); // Mapeia a lista de tags da camada de comunicação para a lista de tags da camada de domínio, garantindo que sejam distintas

            // Mapeia o enum Tag da camada de comunicação para o enum Tag da camada de domínio
            CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(dest => dest.Value, config => config.MapFrom(source => source));
        }

        private void EntityToResponse()
        {
            CreateMap<Expense,ResponseRegisteredExpenseJson>();
            CreateMap<Expense,ResponseShortExpenseJson>();
            CreateMap<Expense, ResponseExpenseJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
