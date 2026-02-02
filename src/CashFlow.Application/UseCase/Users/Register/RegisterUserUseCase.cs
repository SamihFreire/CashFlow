using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Users.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        public readonly IMapper _mapper;

        public RegisterUserUseCase(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            Validate(request);
        }

        private void Validate(RequestRegisterUserJson request)
        {
            var result = new ResgisterUserValidator().Validate(request);
        }
    }
}
