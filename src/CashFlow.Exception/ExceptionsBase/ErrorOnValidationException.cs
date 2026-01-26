using System.Net;

namespace CashFlow.Exception.ExceptionsBase
{
    public class ErrorOnValidationException : CashFlowException
    {
        private readonly List<string> _errors;

        public override int StatusCode => (int)HttpStatusCode.BadRequest;

        // CashFlowException possui um construtor que recebe uma menssagem
        // Aqui mando pro contrutor uma menssagem vazia
        public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
        {
            _errors = errorMessages;
        }

        public override List<string> GetErrors()
        {
            return _errors;
        }
    }
}
