using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Services {
    public class ExpenseServices {

        // preciso terminar o Get futuramente ele precisará do filtros opcionais de id da pessoa, id da subcategoria,
        // se já foi pago (isPaid), se esta atrasado (Duedate)
        // criar um parametro chamado totalAmount onde ele irá pegar o valor de todas as expenses daquele filtro e somar
        // usarei IQueryable
        public async Task<(List<ResponseExpenseDto>, int total)> Get(
            ExpenseControlSystemDataContext context,
            int page,
            int pageSize) {

            int total = await context.Expenses.CountAsync();

            var expenses = await context
                .Expenses
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ResponseExpenseDto {
                    Id = x.Id,
                    Description = x.Description,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    PaidAt = x.PaidAt,
                    IsPaid = x.IsPaid,
                    CreatedAt = x.CreatedAt,
                    UserId = x.UserId,
                    SubCategoryId = x.SubCategoryId,

                })
                .ToListAsync();

            return (expenses, total);
        }

        public async Task<ServiceResult<ResponseExpenseDto>> GetById(
            ExpenseControlSystemDataContext context,
            Guid id) {

            var expenses = await context
                .Expenses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (expenses == null) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x03 - Despesa não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            return new ServiceResult<ResponseExpenseDto> {
                Success = true,
                Result = new ResponseExpenseDto {
                    Id = expenses.Id,
                    Description = expenses.Description,
                    Amount = expenses.Amount,
                    DueDate = expenses.DueDate,
                    PaidAt = expenses.PaidAt,
                    IsPaid = expenses.IsPaid,
                    CreatedAt = expenses.CreatedAt,
                    UserId = expenses.UserId,
                    SubCategoryId = expenses.SubCategoryId,
                }
            };
        }

        public async Task<ServiceResult<ResponseExpenseDto>> Post(
            ExpenseControlSystemDataContext context,
            PostExpenseDto dto) {

            var userExists = await context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.UserId);

            if (!userExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var subCategoryExists = await context.SubCategories
                .AsNoTracking()
                .AnyAsync(x => x.Id == dto.SubCategoryId);

            if (!subCategoryExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            if (dto.PaidAt != null && dto.PaidAt > DateTime.Now) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "PaidAt não pode conter uma data futura",
                    ClientErrorStatusCode = EClientErrorStatusCode.BadRequest
                };
            }

            if (dto.IsPaid == true && dto.PaidAt == null) {
                dto.PaidAt = DateTime.Now;
            }

            if (dto.PaidAt != null) {
                dto.IsPaid = true;
            }
            else {
                dto.IsPaid = false;
            }

            var expense = new Expense {
                Description = dto.Description,
                Amount = dto.Amount!.Value,
                DueDate = dto.DueDate!.Value,
                PaidAt = dto.PaidAt,
                IsPaid = dto.IsPaid,
                SubCategoryId = dto.SubCategoryId!.Value,
                UserId = dto.UserId!.Value,
            };

            await context.AddAsync(expense);
            await context.SaveChangesAsync();


            return new ServiceResult<ResponseExpenseDto> {
                Success = true,
                Result = new ResponseExpenseDto {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    DueDate = expense.DueDate,
                    PaidAt = expense.PaidAt,
                    IsPaid = expense.IsPaid,
                    CreatedAt = expense.CreatedAt,
                    UserId = expense.UserId,
                    SubCategoryId = expense.SubCategoryId,
                }
            };
        }

        public async Task<ServiceResult<ResponseExpenseDto>> Put(
            ExpenseControlSystemDataContext context,
            PutExpenseDto dto,
            Guid id) {

            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense == null) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Despesa não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound,
                };
            }

            var userExists = await context.Users.AsNoTracking().AnyAsync(x => x.Id == dto.UserId);

            if (!userExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var subCategoryExists = await context.SubCategories.AsNoTracking().AnyAsync(x => x.Id == dto.SubCategoryId);

            if (!subCategoryExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            if (dto.Amount <= 0) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "O valor do custo deve ser maior que zero",
                    ClientErrorStatusCode = EClientErrorStatusCode.BadRequest
                };
            }

            if (dto.PaidAt != null && dto.PaidAt > DateTime.Now) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "PaidAt não pode conter uma data futura",
                    ClientErrorStatusCode = EClientErrorStatusCode.BadRequest
                };
            }

            if (dto.IsPaid == true && dto.PaidAt == null) {
                dto.PaidAt = DateTime.Now;
            }

            if (dto.PaidAt != null) {
                dto.IsPaid = true;
            }
            else {
                dto.IsPaid = false;
            }


            expense.Description = dto.Description;
            expense.Amount = dto.Amount!.Value;
            expense.DueDate = dto.DueDate!.Value;
            expense.PaidAt = dto.PaidAt;
            expense.IsPaid = dto.IsPaid;
            expense.SubCategoryId = dto.SubCategoryId!.Value;
            expense.UserId = dto.UserId!.Value;

            context.Update(expense);
            await context.SaveChangesAsync();


            return new ServiceResult<ResponseExpenseDto> {
                Success = true,
                Result = new ResponseExpenseDto {
                    Id = expense.Id,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    DueDate = expense.DueDate,
                    PaidAt = expense.PaidAt,
                    IsPaid = expense.IsPaid,
                    CreatedAt = expense.CreatedAt,
                    UserId = expense.UserId,
                    SubCategoryId = expense.SubCategoryId,
                }
            };
        }

        public async Task<ServiceResult<ResponseExpenseDto>> Delete(
            ExpenseControlSystemDataContext context,
            Guid id) {

            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense == null) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "Despesa não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }
            context.Remove(expense);
            await context.SaveChangesAsync();


            return new ServiceResult<ResponseExpenseDto> {
                Success = true,
            };
        }
    }
}
