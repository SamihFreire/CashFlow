namespace CashFlow.Communication.Responses
{
    public class responseErrorJson
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public responseErrorJson(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
