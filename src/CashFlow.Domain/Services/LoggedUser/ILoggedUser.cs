using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> Get();
    }
}
