using CashFlow.Domain.Repositories.Expenses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories
{
    public class ExpensesWriteOnlyRepositoryBuilder
    {
        public static IExpensesWriteOnlyRepository Build()
        {
            var mock = new Mock<IExpensesWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
