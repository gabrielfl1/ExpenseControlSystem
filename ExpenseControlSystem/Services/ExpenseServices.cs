using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Services {
    public class ExpenseServices {

        private readonly ExpenseControlSystemDataContext _context;

        public ExpenseServices(ExpenseControlSystemDataContext context) {
            _context = context;
        }

        public async Task<(List<ResponseExpenseDto>, int total, decimal totalAmount)> Get(
            GetExpenseDto dto) {

            IQueryable<Expense> expenseQuery = _context.Expenses.AsNoTracking();

            if (dto.UserId.HasValue) {
                expenseQuery = expenseQuery.Where(x => x.UserId == dto.UserId.Value);
            }

            if (dto.SubCategoryId.HasValue) {
                expenseQuery = expenseQuery.Where(x => x.SubCategoryId == dto.SubCategoryId.Value);
            }

            if (dto.IsPaid.HasValue) { 
                expenseQuery = expenseQuery.Where(x => x.IsPaid == dto.IsPaid.Value);
            }
            
            if (dto.LatePayment.HasValue) { 
                if (dto.LatePayment == true) {
                    expenseQuery = expenseQuery.Where(x => x.IsPaid == false && x.DueDate < DateTime.Now);
                }
                else {
                    expenseQuery = expenseQuery.Where(x => x.IsPaid == true || x.DueDate >= DateTime.Now);
                }
                
            }

            int total = await expenseQuery.CountAsync();

            decimal totalAmount = await expenseQuery.SumAsync(x => x.Amount);

            var expenses = await expenseQuery
                .AsNoTracking()
                .Skip((dto.Page!.Value - 1) * dto.PageSize!.Value)
                .Take(dto.PageSize!.Value)
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

            return (expenses, total, totalAmount);
        }

        public async Task<ServiceResult<ResponseExpenseDto>> GetById(
            Guid id) {

            var expenses = await _context
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
            PostExpenseDto dto) {

            var userExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.UserId);

            if (!userExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x06 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var subCategoryExists = await _context.SubCategories
                .AsNoTracking()
                .AnyAsync(x => x.Id == dto.SubCategoryId);

            if (!subCategoryExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x07 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            if (dto.PaidAt != null && dto.PaidAt > DateTime.Now) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x08 - PaidAt não pode conter uma data futura",
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

            await _context.AddAsync(expense);
            await _context.SaveChangesAsync();


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
            PutExpenseDto dto,
            Guid id) {

            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense == null) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x11 - Despesa não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound,
                };
            }

            var userExists = await _context.Users.AsNoTracking().AnyAsync(x => x.Id == dto.UserId);

            if (!userExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x12 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            var subCategoryExists = await _context.SubCategories.AsNoTracking().AnyAsync(x => x.Id == dto.SubCategoryId);

            if (!subCategoryExists) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x13 - Subcategoria não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            if (dto.Amount <= 0) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x14 - O valor do custo deve ser maior que zero",
                    ClientErrorStatusCode = EClientErrorStatusCode.BadRequest
                };
            }

            if (dto.PaidAt != null && dto.PaidAt > DateTime.Now) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x15 - PaidAt não pode conter uma data futura",
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

            _context.Update(expense);
            await _context.SaveChangesAsync();


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
            Guid id) {

            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense == null) {
                return new ServiceResult<ResponseExpenseDto> {
                    Success = false,
                    Error = "04x18 - Despesa não encontrada",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }
            _context.Remove(expense);
            await _context.SaveChangesAsync();


            return new ServiceResult<ResponseExpenseDto> {
                Success = true,
            };
        }
    }
}
