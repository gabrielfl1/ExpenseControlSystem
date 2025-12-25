using ExpenseControlSystem.DTOs.ExpenseDtos;
using System.Text.Json.Serialization;

namespace ExpenseControlSystem.DTOs.UserDtos {
    public class ResponseUserDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ResponseExpenseDto>? Expenses { get; set; }
    }
}