using ClosedXML.Excel;
using ExpenseControlSystem.Data;
using ExpenseControlSystem.DTOs.SendEmailDto;
using ExpenseControlSystem.DTOs.UserDtos;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Services {
    public class GenerateXlsx {

        private readonly ExpenseControlSystemDataContext _context;

        public GenerateXlsx(ExpenseControlSystemDataContext context) {
            _context = context;
        }

        public async Task<(ServiceResult<ResponseUserDto>, string? document)> GenerateXlsxAsync(SendEmailDto dto) {

            IQueryable<Expense> expenseQuery = _context.Expenses.AsNoTracking();

            if (dto.SubCategoryId != null && dto.SubCategoryId.Count > 0) {
                expenseQuery = expenseQuery.Where(x => dto.SubCategoryId.Contains(x.SubCategoryId));
            }

            if (dto.UserId != null && dto.UserId.Count > 0) {
                expenseQuery = expenseQuery.Where(x => dto.UserId.Contains(x.UserId));
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

            if (!await expenseQuery.AnyAsync()) {
                return (
                    new ServiceResult<ResponseUserDto> {
                        Success = false,
                        Error = "02x20 - Não há informações para gerar relatório com este filtro",
                        ClientErrorStatusCode = Enums.EErrorStatusCode.NotFound
                    },
                    null
                );
            }

            // criando Excell 
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Relatorio");

            ws.Cell(1, 1).Value = "UserId";
            ws.Cell(1, 2).Value = "SubCategoryId";
            ws.Cell(1, 3).Value = "Amount";
            ws.Cell(1, 4).Value = "DueDate";
            ws.Cell(1, 5).Value = "PaidAt";
            ws.Cell(1, 6).Value = "IsPaid";

            var expenses = await expenseQuery.ToListAsync();

            int row = 2;

            foreach (var item in expenses) {
                ws.Cell(row, 1).Value = item.UserId.ToString();
                ws.Cell(row, 2).Value = item.SubCategoryId.ToString();
                ws.Cell(row, 3).Value = item.Amount;
                ws.Cell(row, 4).Value = item.DueDate;
                ws.Cell(row, 5).Value = item.PaidAt;
                ws.Cell(row, 6).Value = item.IsPaid;

                row++;
            }

            decimal totalAmount = expenses.Sum(x => x.Amount);

            ws.Cell(row, 1).Value = "Total de gastos";
            ws.Cell(row, 3).Value = totalAmount;
            ws.Cell(row, 1).Style.Font.Bold = true;
            ws.Cell(row, 3).Style.Font.Bold = true;

            ws.Columns().AdjustToContents();

            byte[] document;

            using (var stream = new MemoryStream()) {
                wb.SaveAs(stream);
                document = stream.ToArray();
            }

            string base64Document = Convert.ToBase64String(document);


            return (
                new ServiceResult<ResponseUserDto> { Success = true },
                base64Document
            );
        }
    }
}
