using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.Reports.Pdf
{
    public interface IGenerateExpenseReportPdfUseCase
    {
        public Task<byte[]> Execute(DateOnly month);
    }
}
