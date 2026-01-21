using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        // Realizando a injeção de dependencia via [FromServices]
        [HttpPost]
        public IActionResult Register([FromServices] IRegisterExpenseUseCase useCase, [FromBody] RequestRegisterExpenseJson request)
        {
            var response = useCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
