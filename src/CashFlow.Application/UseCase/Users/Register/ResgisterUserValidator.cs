using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Users.Register
{
    public class ResgisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public ResgisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
                .EmailAddress()
                .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        }
    }
}
