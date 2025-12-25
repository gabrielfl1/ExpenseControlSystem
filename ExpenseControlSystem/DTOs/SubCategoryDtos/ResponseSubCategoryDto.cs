using ExpenseControlSystem.DTOs.ExpenseDtos;

namespace ExpenseControlSystem.DTOs.SubCategoryDtos {
    public class ResponseSubCategoryDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ResponseExpenseDto>? Expenses { get; set; }
    }
}
