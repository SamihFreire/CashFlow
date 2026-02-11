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
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
                .EmailAddress() // Validando se o email é válido, mas somente se ele não for vazio, para evitar mensagens de erro redundantes (Por conta do When abaixo)
                .When(user => string.IsNullOrWhiteSpace(user.Email) == false, applyConditionTo: ApplyConditionTo.CurrentValidator) // O ApplyConditionTo.CurrentValidator é utilizado para aplicar a condição somente para o validador atual, ou seja, o EmailAddress, e não para os outros validadores, como o NotEmpty, para evitar mensagens de erro redundantes
                .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        }
    }
}
