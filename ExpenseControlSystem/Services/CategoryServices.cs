using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.CategoryDtos;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Enums;

namespace ExpenseControlSystem.Services {
    public class CategoryServices {

        public async Task<(List<ResponseCategoryDto>, int total)> Get(
            ExpenseControlSystemDataContext context,
            int page,
            int pageSize) {

            var total = await context.Categories.CountAsync();

            var categories = await context
                .Categories
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ResponseCategoryDto {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();

            return (categories, total);
        }

        public async Task<ServiceResult<ResponseCategoryDto>> GetById(
            ExpenseControlSystemDataContext context,
            Guid id) {

            var category = await context
                .Categories
                .AsNoTracking()
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x03 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
                Result = new ResponseCategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    SubCategories = category.SubCategories.Select(sub => new DTOs.SubCategoryDtos.ResponseSubCategoryDto {
                        Id = sub.Id,
                        Name = sub.Name,
                        Description = sub.Description,
                    }).ToList()
                }
            };
        }

        public async Task<ServiceResult<ResponseCategoryDto>> Post(
            ExpenseControlSystemDataContext context,
            PostCategoryDto dto) {

            dto.Name = StringExtensions.StringTitleEditor(dto.Name);

            if (await context.Categories.AnyAsync(x => x.Name == dto.Name))
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x06 - Já existe uma categoria com esse nome",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };

            var category = new Category {
                Name = dto.Name,
                Description = dto.Description,
                SubCategories = new List<SubCategory>()
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
                Result = new ResponseCategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }
            };
        }

        public async Task<ServiceResult<ResponseCategoryDto>> Put(
            ExpenseControlSystemDataContext context,
            PutCategoryDto dto,
            Guid id) {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x10 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            dto.Name = StringExtensions.StringTitleEditor(dto.Name);

            if (await context.Categories.AnyAsync(x => x.Name == dto.Name && x.Id != id))
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x11 - Já existe uma categoria com esse nome",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };

            category.Name = dto.Name;
            category.Description = dto.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
                Result = new ResponseCategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }
            };
        }

        public async Task<ServiceResult<ResponseCategoryDto>> Patch(
            ExpenseControlSystemDataContext context,
            PatchCategoryDto dto,
            Guid id) {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x14 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            if (dto.Name != null) {
                dto.Name = StringExtensions.StringTitleEditor(dto.Name);

                if (await context.Categories.AnyAsync(x => x.Name == dto.Name && x.Id != id))
                    return new ServiceResult<ResponseCategoryDto> {
                        Success = false,
                        Error = "01x15 - Já existe uma categoria com esse nome",
                        ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                    };

                category.Name = dto.Name;
            }

            if (dto.Description != null)
                category.Description = dto.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
                Result = new ResponseCategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                }
            };
        }
        public async Task<ServiceResult<ResponseCategoryDto>> Delete(
            ExpenseControlSystemDataContext context,
            Guid id) {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x18 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            context.Categories.Remove(category!);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
            };
        }

    }
}
