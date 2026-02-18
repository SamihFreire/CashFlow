using CashFlow.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Users.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
