using CashFlow.Application.UseCase.Expenses.Reports.Excel;
using CashFlow.Application.UseCase.Expenses.Reports.Pdf;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)] // Somente administradores podem acessar os relatórios
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel(
            [FromServices] IGenerateExpenseReportExcelUseCase useCase,
            [FromHeader] DateOnly month) // Recebendo a data pelo header
        {
            byte[] file = await useCase.Execute(month);

            if(file.Length > 0)
            {
                // Para devolver uma arquivo
                // MediaTypeNames.Application.Octet sinaliza ao navegador que ele não precisa interpretar esse arquivo
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
            }

            return NoContent();
        }

        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdf(
            [FromServices] IGenerateExpenseReportPdfUseCase useCase,
            [FromQuery] DateOnly month) // Vai receber a data pela url
        {
            byte[] file = await useCase.Execute(month);

            if (file.Length > 0)
            {
                // Para devolver uma arquivo
                // MediaTypeNames.Application.Octet sinaliza ao navegador que ele não precisa interpretar esse arquivo
                return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
            }

            return NoContent();
        }
    }
}
