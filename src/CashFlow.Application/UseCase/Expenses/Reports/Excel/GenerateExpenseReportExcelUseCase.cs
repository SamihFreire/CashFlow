using CashFlow.Domain.Reports;
using ClosedXML.Excel;

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


            InserHeader(worksheet);
        }

        private void InserHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F3C2B6");

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
    }
}
