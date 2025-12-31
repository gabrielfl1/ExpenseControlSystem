using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Services;
using ExpenseControlSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace ExpenseControlSystem.Controllers {
    [ApiController]
    [Route("v1/Expenses")]
    public class ExpenseController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetExpenseDto dto,
            [FromServices] ExpenseServices expenseServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (expenses, total, totalAmount) = await expenseServices.Get(context, dto);

                var response = new PagedResultDto<ResponseExpenseDto> {
                    Result = expenses,
                    Total = total,
                    Page = dto.Page!.Value,
                    PageSize = dto.PageSize!.Value,
                    TotalAmount = totalAmount

                };

                return Ok(new ResultViewModel<PagedResultDto<ResponseExpenseDto>>(response));


            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("04x01"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x02"));
            }

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] ExpenseServices expenseServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            try {

                var expense = await expenseServices.GetById(context, id);

                if (!expense.Success) {
                    switch (expense.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(expense.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseExpenseDto>(expense.Result));

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("04x04"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x05"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] PostExpenseDto dto,
            [FromServices] ExpenseServices expenseServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var expense = await expenseServices.Post(context, dto);

                if (!expense.Success) {
                    switch (expense.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(expense.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = expense.Result.Id },
                    new ResultViewModel<ResponseExpenseDto>(expense.Result)
                );

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("04x04"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x05"));
            }

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] PutExpenseDto dto,
            [FromServices] ExpenseServices expenseServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var expense = await expenseServices.Put(context, dto, id);

                if (!expense.Success) {
                    switch (expense.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(expense.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseExpenseDto>(expense.Result));

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("04x04"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x05"));
            }
        }






        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] ExpenseServices expenseServices,
            [FromServices] ExpenseControlSystemDataContext context) { 
            
            try {

                var expense = await expenseServices.Delete(context, id);
                if (!expense.Success) {
                    switch (expense.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(expense.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(expense.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return NoContent();

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("04x04"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x05"));
            }
        }


        // Fazer o Put, Patch
    }
}
