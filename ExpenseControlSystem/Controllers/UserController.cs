using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Models;
using ExpenseControlSystem.Services;
using ExpenseControlSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace ExpenseControlSystem.Controllers {
    [ApiController]
    [Route("v1/users")]

    public class UserController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetUserDto dto,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (users, total) = await userServices.Get(context, dto.Page!.Value, dto.PageSize!.Value);

                var result = new PagedResultDto<ResponseUserDto> {
                    Result = users,
                    Total = total,
                    Page = dto.Page!.Value,
                    PageSize = dto.PageSize!.Value
                };

                return Ok(new ResultViewModel<PagedResultDto<ResponseUserDto>>(result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x01 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x02 - Erro interno de servidor"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromQuery] GetByIdUserDto dto,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await userServices.GetById(context, id, dto);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                var responseDto = new ResponseUserDto {
                    Id = user.Result.Id,
                    Name = user.Result.Name,
                    Email = user.Result.Email,
                    CreatedAt = user.Result.CreatedAt,
                    Expenses = user.Result.Expenses
                };
                return Ok(new ResultViewModel<ResponseUserDto>(responseDto));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x04 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x05 - Erro interno de servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] PostUserDto dto,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await userServices.Post(context, dto);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }
                
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = user.Result.Id },
                    new ResultViewModel<ResponseUserDto>(user.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x07 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x08 - Erro interno de servidor"));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] PutUserDto dto,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await userServices.Put(context, dto, id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseUserDto>(user.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x11 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x12 - Erro interno de servidor"));
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(
            [FromRoute] Guid id,
            [FromBody] PatchUserDto dto,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await userServices.Patch(context, dto, id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseUserDto>(user.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x15 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x16 - Erro interno de servidor"));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] UserServices userServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            try {

                var user = await userServices.Delete(context, id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok();
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x18 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x19 - Erro interno de servidor"));
            }
        }

    }
}
