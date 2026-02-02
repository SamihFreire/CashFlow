using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess
{
    public class CashFlowDbContext : DbContext
    {
        // : base(options) -> Esta repassando para o contrutor da classe base(o DbContext) as options
        public CashFlowDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
