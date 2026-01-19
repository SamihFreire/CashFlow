using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // CashFlowException é a classe Pai que herda System.Exception
            // Qualque classe de erro que herda dela vai cair nesse IF
            if (context.Exception is CashFlowException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknorError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            // Local onde sera mapeado os erros tratados
            if(context.Exception is ErrorOnValidationException)
            {
                var ex = (ErrorOnValidationException)context.Exception;
                var errorResponse = new ResponseErrorJson(ex.Errors);

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(errorResponse);
            }// Caso nao seja referente a nenhum erro tratado cai nesse
            else
            {
                var errorResponse = new ResponseErrorJson(context.Exception.Message);

                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }

        private void ThrowUnknorError(ExceptionContext context)
        {
            var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
