using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel([FromHeader] DateOnly month) // Recebendo a data pelo header
        {
            byte[] file = new byte[1];

            if(file.Length > 0)
            {
                // Para devolver uma arquivo
                // MediaTypeNames.Application.Octet sinaliza ao navegador que ele não precisa interpretar esse arquivo
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
            }

            return NoContent();
        }
    }
}
