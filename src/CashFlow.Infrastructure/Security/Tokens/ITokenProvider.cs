namespace CashFlow.Infrastructure.Security.Tokens
{
    public interface ITokenProvider
    {
        public string TokenOnRequest();
    }
}
