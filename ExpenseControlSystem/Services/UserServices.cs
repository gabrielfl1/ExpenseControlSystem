using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Enums;
using ExpenseControlSystem.Extensions;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Services {
    public class UserServices {

        public async Task<(List<ResponseUserDto>, int total)> Get(
            ExpenseControlSystemDataContext context,
            int page,
            int pageSize) {

            int total = await context.Users.CountAsync();

            var users = await context
                .Users
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new ResponseUserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                })
                .ToListAsync();

            return (users, total);
        }

        public async Task<ServiceResult<ResponseUserDto>> GetById(
            ExpenseControlSystemDataContext context,
            Guid id,
            GetByIdUserDto dto) {

            var user = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) {
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x03 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };
            }

            IQueryable<Expense> expensesQuery = context
                .Expenses
                .AsNoTracking()
                .Where(x => x.UserId == id);

            if (dto.IsPaid.HasValue) {
                expensesQuery = expensesQuery.Where(x => x.IsPaid == dto.IsPaid.Value);
            }

            var expenses = await expensesQuery.Select(x => new ResponseExpenseDto {
                Id = x.Id,
                Description = x.Description,
                Amount = x.Amount,
                DueDate = x.DueDate,
                PaidAt = x.PaidAt,
                IsPaid = x.IsPaid,
                CreatedAt = x.CreatedAt,
            }).ToListAsync();

            return new ServiceResult<ResponseUserDto> {
                Success = true,
                Result = new ResponseUserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Expenses = expenses
                }
            };
        }

        public async Task<ServiceResult<ResponseUserDto>> Post(
            ExpenseControlSystemDataContext context,
            PostUserDto dto) {

            dto.Name = StringExtensions.StringNameEditor(dto.Name);
            dto.Email = dto.Email.Trim().ToLower();

            if (await context.Users.AnyAsync(x => x.Email == dto.Email))
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x06 - Não é possivel cadastrar um usuario com este E-mail",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };

            var user = new User {
                Name = dto.Name,
                Email = dto.Email,
            };

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseUserDto> {
                Success = true,
                Result = new ResponseUserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                }
            };
        }

        public async Task<ServiceResult<ResponseUserDto>> Put(
            ExpenseControlSystemDataContext context,
            PutUserDto dto,
            Guid id) {

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x09 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            dto.Email = dto.Email.Trim().ToLower();

            if (await context.Users.AnyAsync(x => x.Email == dto.Email && x.Id != id))
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x10 - Não é possivel atualizar um usuario com este E-mail",
                    ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                };

            dto.Name = StringExtensions.StringNameEditor(dto.Name);

            user.Name = dto.Name;
            user.Email = dto.Email;

            context.Update(user);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseUserDto> {
                Success = true,
                Result = new ResponseUserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                }
            };
        }

        public async Task<ServiceResult<ResponseUserDto>> Patch(
            ExpenseControlSystemDataContext context,
            PatchUserDto dto,
            Guid id) {

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x13 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            if (!string.IsNullOrWhiteSpace(dto.Name)) {
                dto.Name = StringExtensions.StringNameEditor(dto.Name);
                user.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Email)) {
                dto.Email = dto.Email.Trim().ToLower();

                if (await context.Users.AnyAsync(x => x.Email == dto.Email && x.Id != id))
                    return new ServiceResult<ResponseUserDto> {
                        Success = false,
                        Error = "02x14 - Não é possivel atualizar um usuario com este E-mail",
                        ClientErrorStatusCode = EClientErrorStatusCode.Conflict
                    };

                user.Email = dto.Email;
            }

            context.Update(user);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseUserDto> {
                Success = true,
                Result = new ResponseUserDto {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                }
            };
        }

        public async Task<ServiceResult<ResponseUserDto>> Delete(
            ExpenseControlSystemDataContext context,
            Guid id) {

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return new ServiceResult<ResponseUserDto> {
                    Success = false,
                    Error = "02x17 - Usuário não encontrado",
                    ClientErrorStatusCode = EClientErrorStatusCode.NotFound
                };

            context.Remove(user);
            await context.SaveChangesAsync();

            return new ServiceResult<ResponseUserDto> {
                Success = true
            };
        }
    }
}
