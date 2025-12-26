using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs;
using ExpenseControlSystem.DTOs.CategoryDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Models;
using ExpenseControlSystem.Services;
using ExpenseControlSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ExpenseControlSystem.Controllers {
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetCategoryDto dto,
            [FromServices] CategoryServices categoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try {

                var (categories, total) = await categoryServices.Get(context, dto.Page!.Value, dto.PageSize!.Value);

                var result = new PagedResultDto<ResponseCategoryDto> {
                    Result = categories,
                    Total = total,
                    Page = dto.Page.Value,
                    PageSize = dto.PageSize.Value
                };

                return Ok(new ResultViewModel<PagedResultDto<ResponseCategoryDto>>(result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x01 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x02 - Erro interno de servidor"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] CategoryServices categoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            try {

                var category = await categoryServices.GetById(context, id);

                if (!category.Success) {
                    switch (category.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(category.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }
                return Ok(new ResultViewModel<ResponseCategoryDto>(category.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x04 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x05 - Erro interno de servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromServices] ExpenseControlSystemDataContext context,
            [FromServices] CategoryServices categoryServices,
            [FromBody] PostCategoryDto dto) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try {
                var category = await categoryServices.Post(context, dto);

                if (!category.Success) {
                    switch (category.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(category.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = category.Result.Id },
                    new ResultViewModel<ResponseCategoryDto>(category.Result));

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x08 Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x09 Erro interno de servidor"));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] PutCategoryDto dto,
            [FromServices] CategoryServices categoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try {
                var category = await categoryServices.Put(context, dto, id);

                if (!category.Success) {
                    switch (category.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(category.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseCategoryDto>(category.Result));

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x12 Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x13 Erro interno de servidor"));
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(
            [FromRoute] Guid id,
            [FromBody] PatchCategoryDto dto,
            [FromServices] CategoryServices categoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }
            try {

                var category = await categoryServices.Patch(context, dto, id);

                if (!category.Success) {
                    switch (category.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(category.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseCategoryDto>(category.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x16 Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x17 Erro interno de servidor"));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] CategoryServices categoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            try {
                var category = await categoryServices.Delete(context, id);

                if (!category.Success) {
                    switch (category.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(category.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(category.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return NoContent();
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("01x19 Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("01x20 Erro interno de servidor"));
            }
        }
    }
}
