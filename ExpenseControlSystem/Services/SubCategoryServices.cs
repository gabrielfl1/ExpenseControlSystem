using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using ExpenseControlSystem.DTOs.SubCategoryDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Services {
    public class SubCategoryServices {

        private readonly ExpenseControlSystemDataContext _context;

        public SubCategoryServices(ExpenseControlSystemDataContext context) {
            _context = context;
        }

        public async Task<(List<ResponseSubCategoryDto>, int total)> Get(
            GetSubCategoryDto dto) {

            int total = await _context.SubCategories.CountAsync();

            var subCategories = await _context
                .SubCategories
                .AsNoTracking()
                .Skip((dto.Page!.Value - 1) * dto.PageSize!.Value)
                .Take(dto.PageSize!.Value)
                .Select(x => new ResponseSubCategoryDto {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryId = x.CategoryId,
                })
                .ToListAsync();

            return (subCategories, total);
        }

        public async Task<ServiceResult<ResponseSubCategoryDto>> GetById(
            GetByIdSubCategoryDto dto,
            Guid id) {

            var subCategory = await _context
                .SubCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (subCategory == null) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x03 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            IQueryable<Expense> expenseQuery = _context
                .Expenses
                .AsNoTracking()
                .Where(x => x.SubCategoryId == subCategory.Id);

            if (dto.IsPaid.HasValue) {
                expenseQuery = expenseQuery.Where(x => x.IsPaid == dto.IsPaid.Value);
            }

            var expense = await expenseQuery.Select(x => new ResponseExpenseDto {
                Id = x.Id,
                Description = x.Description,
                Amount = x.Amount,
                DueDate = x.DueDate,
                PaidAt = x.PaidAt,
                IsPaid = x.IsPaid,
                CreatedAt = x.CreatedAt,
            }).ToListAsync();

            return new ServiceResult<ResponseSubCategoryDto> {
                Success = true,
                Result = new ResponseSubCategoryDto {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Description = subCategory.Description,
                    CategoryId = subCategory.CategoryId,
                    Expenses = expense
                }
            };
        }

        public async Task<ServiceResult<ResponseSubCategoryDto>> Post(
            PostSubCategoryDto dto) {

            dto.Name = StringExtensions.StringNameEditor(dto.Name);

            if (await _context.SubCategories.AnyAsync(x => x.Name == dto.Name && x.CategoryId == dto.CategoryId)) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x06 - Já existe uma subcategoria com esse nome nesta categoria",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };
            }

            var categoryExists = await _context
                .Categories
                .AsNoTracking()
                .AnyAsync(x => x.Id == dto.CategoryId);

            if (!categoryExists) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x07 - Não existe uma categoria com esse Id para ser adcionada há uma subcategoria",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var category = new SubCategory {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId!.Value
            };

            await _context.SubCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new ServiceResult<ResponseSubCategoryDto> {
                Success = true,
                Result = new ResponseSubCategoryDto {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CategoryId = category.CategoryId
                }
            };
        }

        public async Task<ServiceResult<ResponseSubCategoryDto>> Put(
            PutSubCategoryDto dto,
            Guid id) {

            var subCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (subCategory == null) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x10 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var categoryExists = await _context
                .Categories
                .AsNoTracking()
                .AnyAsync(x => x.Id == dto.CategoryId);

            if (!categoryExists) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x11 - Não existe uma categoria com esse Id para ser adcionada há uma subcategoria",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            dto.Name = StringExtensions.StringNameEditor(dto.Name);

            if (await _context
                .SubCategories
                .AnyAsync(x => x.Name == dto.Name && x.CategoryId == dto.CategoryId && x.Id != id)) {

                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x12 - Já existe uma subcategoria com esse nome nesta categoria",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };
            }

            subCategory.Name = dto.Name;
            subCategory.Description = dto.Description;
            subCategory.CategoryId = dto.CategoryId!.Value;


            _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();

            return new ServiceResult<ResponseSubCategoryDto> {
                Success = true,
                Result = new ResponseSubCategoryDto {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Description = subCategory.Description,
                    CategoryId = subCategory.CategoryId
                }
            };
        }

        public async Task<ServiceResult<ResponseSubCategoryDto>> Patch(
            PatchSubCategoryDto dto,
            Guid id) {

            var subCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (subCategory == null) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x15 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            if (dto.CategoryId != null) {

                var categoryExists = await _context
                    .Categories
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == dto.CategoryId);

                if (!categoryExists) {
                    return new ServiceResult<ResponseSubCategoryDto> {
                        Success = false,
                        Error = "03x16 - Não existe uma categoria com esse Id para ser adcionada há uma subcategoria",
                        ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                    };
                }

                subCategory.CategoryId = dto.CategoryId!.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name)) {
                dto.Name = StringExtensions.StringNameEditor(dto.Name);

                var categoryIdToCheck = dto.CategoryId ?? subCategory.CategoryId;

                if (await _context
                    .SubCategories
                    .AnyAsync(x => x.Name == dto.Name && x.CategoryId == categoryIdToCheck && x.Id != id)) {

                    return new ServiceResult<ResponseSubCategoryDto> {
                        Success = false,
                        Error = "03x17 - Já existe uma subcategoria com esse nome nesta categoria",
                        ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                    };
                }
                subCategory.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description)) {
                subCategory.Description = dto.Description;
            }

            _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();

            return new ServiceResult<ResponseSubCategoryDto> {
                Success = true,
                Result = new ResponseSubCategoryDto {
                    Id = subCategory.Id,
                    Name = subCategory.Name,
                    Description = subCategory.Description,
                    CategoryId = subCategory.CategoryId
                }
            };
        }

        public async Task<ServiceResult<ResponseSubCategoryDto>> Delete(
            Guid id) {

            var subCategory = await _context.SubCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (subCategory == null) {
                return new ServiceResult<ResponseSubCategoryDto> {
                    Success = false,
                    Error = "03x20 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();

            return new ServiceResult<ResponseSubCategoryDto> {
                Success = true
            };
        }

    }
}
