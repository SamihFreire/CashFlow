using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserReadOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public UserRepository(CashFlowDbContext dbContex) => _dbContext = dbContex;

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }
    }
}
