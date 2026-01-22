namespace CashFlow.Exception.ExceptionsBase
{
    public class ErrorOnValidationException : CashFlowException
    {
        public List<string> Errors { get; set; }

        // CashFlowException possui um construtor que recebe uma menssagem
        // Aqui mando pro contrutor uma menssagem vazia
        public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
        {
            Errors = errorMessages;
        }
    }
}
