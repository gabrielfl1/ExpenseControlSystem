using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.CategoryDtos;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Enums;

namespace ExpenseControlSystem.Services {
    public class CategoryServices {

        private readonly ExpenseControlSystemDataContext _context;

        public CategoryServices(ExpenseControlSystemDataContext dataContext) {
            _context = dataContext;
        }

        public async Task<(List<ResponseCategoryDto>, int total)> Get(
            GetCategoryDto dto) {

            var total = await _context.Categories.CountAsync();

            var categories = await _context
                .Categories
                .AsNoTracking()
                .Skip((dto.Page!.Value - 1) * dto.PageSize!.Value)
                .Take(dto.PageSize!.Value)
                .Select(x => new ResponseCategoryDto {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();

            return (categories, total);
        }

        public async Task<ServiceResult<ResponseCategoryDto>> GetById(
            Guid id) {

            var category = await _context
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
            PostCategoryDto dto) {

            dto.Name = StringExtensions.StringTitleEditor(dto.Name);

            if (await _context.Categories.AnyAsync(x => x.Name == dto.Name))
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

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

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
            PutCategoryDto dto,
            Guid id) {

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x10 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            dto.Name = StringExtensions.StringTitleEditor(dto.Name);

            if (await _context.Categories.AnyAsync(x => x.Name == dto.Name && x.Id != id))
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x11 - Já existe uma categoria com esse nome",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };

            category.Name = dto.Name;
            category.Description = dto.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

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
            PatchCategoryDto dto,
            Guid id) {

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x14 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            if (!string.IsNullOrWhiteSpace(dto.Name)) {
                dto.Name = StringExtensions.StringTitleEditor(dto.Name);

                if (await _context.Categories.AnyAsync(x => x.Name == dto.Name && x.Id != id))
                    return new ServiceResult<ResponseCategoryDto> {
                        Success = false,
                        Error = "01x15 - Já existe uma categoria com esse nome",
                        ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                    };

                category.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
                category.Description = dto.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

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
            Guid id) {

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return new ServiceResult<ResponseCategoryDto> {
                    Success = false,
                    Error = "01x18 - Categoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            _context.Categories.Remove(category!);
            await _context.SaveChangesAsync();

            return new ServiceResult<ResponseCategoryDto> {
                Success = true,
            };
        }

    }
}
