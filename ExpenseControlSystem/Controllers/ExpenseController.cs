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

        private readonly ExpenseServices _expenseServices;

        public ExpenseController(ExpenseServices expenseServices) { 
            _expenseServices = expenseServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetExpenseDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (expenses, total, totalAmount) = await _expenseServices.Get(dto);

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
                return StatusCode(500, new ResultViewModel<string>("04x01 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x02 - Erro interno de servidor"));
            }

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id) {

            try {

                var expense = await _expenseServices.GetById(id);

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
                return StatusCode(500, new ResultViewModel<string>("04x04 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x05 - Erro interno de servidor"));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] PostExpenseDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var expense = await _expenseServices.Post(dto);

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
                return StatusCode(500, new ResultViewModel<string>("04x09 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x10 - Erro interno de servidor"));
            }

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] PutExpenseDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var expense = await _expenseServices.Put(dto, id);

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
                return StatusCode(500, new ResultViewModel<string>("04x16 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x17 - Erro interno de servidor"));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id) { 
            
            try {

                var expense = await _expenseServices.Delete(id);
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
                return StatusCode(500, new ResultViewModel<string>("04x19 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("04x20 - Erro interno de servidor"));
            }
        }
    }
}
