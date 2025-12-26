using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs;
using ExpenseControlSystem.DTOs.SubCategoryDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Models;
using ExpenseControlSystem.Services;
using ExpenseControlSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace ExpenseControlSystem.Controllers {
    [ApiController]
    [Route("v1/subcategories")]
    public class SubCategoryController : ControllerBase {

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] GetSubCategoryDto dto,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var (subCategories, total) = await subCategoryServices.Get(context, dto.Page!.Value, dto.PageSize!.Value);
                
                var result = new PagedResultDto<ResponseSubCategoryDto> {
                    Result = subCategories,
                    Total = total,
                    Page = dto.Page!.Value,
                    PageSize = dto.PageSize!.Value
                };

                return Ok(new ResultViewModel<PagedResultDto<ResponseSubCategoryDto>>(result));

            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x01 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x02 - Erro interno de servidor"));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromQuery] GetByIdSubCategoryDto dto,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var subCategory = await subCategoryServices.GetById(context, dto, id);

                if (!subCategory.Success) {
                    switch (subCategory.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseSubCategoryDto>(subCategory.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x04 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x05 - Erro interno de servidor"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] PostSubCategoryDto dto,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext context) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {

                var subCategory = await subCategoryServices.Post(context, dto);

                if (!subCategory.Success) {
                    switch (subCategory.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = subCategory.Result.Id },
                    new ResultViewModel<ResponseSubCategoryDto>(subCategory.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x08 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x09 - Erro interno de servidor"));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] PutSubCategoryDto dto,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext contetext) {

            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {
                var subCategory = await subCategoryServices.Put(contetext, dto, id);

                if (!subCategory.Success) {
                    switch (subCategory.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseSubCategoryDto>(subCategory.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x13 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x14 - Erro interno de servidor"));
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(
            [FromRoute] Guid id,
            [FromBody] PatchSubCategoryDto dto,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext contetext) {
            if (!ModelState.IsValid) {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            try {
                var subCategory = await subCategoryServices.Patch(contetext, dto, id);

                if (!subCategory.Success) {
                    switch (subCategory.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return Ok(new ResultViewModel<ResponseSubCategoryDto>(subCategory.Result));
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x18 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x19 - Erro interno de servidor"));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            [FromServices] SubCategoryServices subCategoryServices,
            [FromServices] ExpenseControlSystemDataContext contetext) {

            try {

                var subCategory = await subCategoryServices.Delete(contetext, id);

                if (!subCategory.Success) {
                    switch (subCategory.ClientErrorStatusCode) {
                        case EClientErrorStatusCode.NotFound:
                            return NotFound(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.Conflict:
                            return Conflict(new ResultViewModel<string>(subCategory.Error));
                        case EClientErrorStatusCode.BadRequest:
                            return BadRequest(new ResultViewModel<string>(subCategory.Error));
                        default:
                            return StatusCode(500, new ResultViewModel<string>("Erro inesperado"));
                    }
                }

                return NoContent();
            }
            catch (DbException) {
                return StatusCode(500, new ResultViewModel<string>("03x21 - Erro ao tentar se conectar ao banco de dados"));
            }
            catch (Exception) {
                return StatusCode(500, new ResultViewModel<string>("03x22 - Erro interno de servidor"));
            }
        }
    }
}
