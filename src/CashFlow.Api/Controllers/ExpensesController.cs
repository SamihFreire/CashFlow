using CashFlow.Application.UseCase.Expenses.Delete;
using CashFlow.Application.UseCase.Expenses.GetAll;
using CashFlow.Application.UseCase.Expenses.GetById;
using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Application.UseCase.Expenses.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        // Realizando a injeção de dependencia via [FromServices]
        [HttpPost]
        // Registrando na documentacao os tipos de retorno
        [ProducesResponseType(typeof(ResponseRegisteredExpenseJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromServices] IRegisterExpenseUseCase useCase, [FromBody] RequestExpenseJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpenseUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Expenses.Count != 0)
                return Ok(response);

            return NotFound();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromServices] IGetExpenseByUdUseCase useCase, [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromServices] IDeleteExpenseUseCase useCase, [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }


        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)] // Dadados da despesa invalidos
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)] // ID invalido
        public async Task<IActionResult> Update([FromServices] IUpdateExpenseUseCase useCase,
                                                [FromRoute] long id,
                                                [FromBody] RequestExpenseJson request)
        {
            await useCase.Execute(id, request);

            return NoContent();
        }
    }
}
