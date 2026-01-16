using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.Register
{
    public class RegisterExpenseUseCase
    {
        public ResponseRegisteredExpenseJson Execute(RequestRegisterExpenseJson request)
        {
            Validate(request);

            return new ResponseRegisteredExpenseJson();
        }

        private void Validate(RequestRegisterExpenseJson request)
        {
            // Utilizando a biblioteca do FluentValidation para realizar as validações
            // Criado a classe RegisterExpenseValidator onde foram registradas as validações que serão realizadas
            var validator = new RegisterExpenseValidator();

            // Função da biblioteca que realiza as validações criadas
            var result = validator.Validate(request);

            // Caso possua erros de validação
            // Obtem a lista de erros do FluentValidation e lança a exceção para tratamento
            // ErrorOnValidationException recebe a lista de erros e todo erro cai na ExceptionFilter para tratamento onde estao os erros mapeados
            if (!result.IsValid)
            {
                var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
