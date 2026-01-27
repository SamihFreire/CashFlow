using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCase.Expenses.Reports.Excel
{
    public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
    {
        public async Task<byte[]> Execute(DateOnly month)
        {
            // Utilizando o pacote ClosedXML para geração de arquivos XML
            var workbook =  new XLWorkbook();
            workbook.Author = "Samih Freire";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        }
    }
}
