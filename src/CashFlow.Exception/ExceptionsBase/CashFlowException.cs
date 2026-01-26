namespace CashFlow.Exception.ExceptionsBase
{
    // Essa classe nao pode ser instanciada por conta do abstract
    public abstract class CashFlowException : System.Exception 
    {
        protected CashFlowException(string message) : base(message)
        {
            
        }

        public abstract int StatusCode { get; }
        public abstract List<string> GetErrors();
    }
}
