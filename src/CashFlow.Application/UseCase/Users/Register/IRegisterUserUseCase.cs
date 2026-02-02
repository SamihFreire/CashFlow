using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Users.Register
{
    public interface IRegisterUserUseCase
    {
        public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
