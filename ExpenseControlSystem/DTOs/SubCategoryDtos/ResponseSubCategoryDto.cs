using ExpenseControlSystem.DTOs.CategoryDtos;
using ExpenseControlSystem.DTOs.ExpenseDtos;
using System.Text.Json.Serialization;

namespace ExpenseControlSystem.DTOs.SubCategoryDtos {
    public class ResponseSubCategoryDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ResponseExpenseDto>? Expenses { get; set; }
    }
}
