using ExpenseControlSystem.DTOs;
using ExpenseControlSystem.DTOs.SendEmailDto;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Interfaces;
using ExpenseControlSystem.Services;
using ExpenseControlSystem.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace ExpenseControlSystem.Controllers {
    [ApiController]
    [Route("v1/users")]

    public class UserController : ControllerBase {

        private readonly UserServices _userServices;

        public UserController(UserServices userServices) {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetUserDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (users, total) = await _userServices.Get(dto);

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
            [FromQuery] GetByIdUserDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await _userServices.GetById(id, dto);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseUserDto>(user.Result));
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
            [FromBody] PostUserDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await _userServices.Post(dto);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.BadRequest:
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
            [FromBody] PutUserDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await _userServices.Put(dto, id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.BadRequest:
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
            [FromBody] PatchUserDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var user = await _userServices.Patch(dto, id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.BadRequest:
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
            [FromRoute] Guid id) {

            try {

                var user = await _userServices.Delete(id);

                if (!user.Success) {
                    switch (user.ClientErrorStatusCode) {
                        case EErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(user.Error));
                        case EErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(user.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return NoContent();
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x18 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x19 - Erro interno de servidor"));
            }
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendMail(
            [FromBody] SendEmailDto dto,
            [FromServices] IEmailServices emailServices,
            [FromServices] GenerateXlsx xlsxServices) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (result, document) = await xlsxServices.GenerateXlsxAsync(dto);

                if (!result.Success) {
                    return NotFound(new ResultViewModel<string>(result.Error));
                }

                var success = await emailServices.SendEmail(dto, document);

                if (!success.Success) {
                    return StatusCode(success.Result.StatusCode!.Value, new ResultViewModel<string>(success.Result.Message!));
                }


                return Ok(new ResultViewModel<ResponseSendEmailDto>(success.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("02x21 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("02x22 - Erro interno de servidor"));
            }
        }
    }
}
