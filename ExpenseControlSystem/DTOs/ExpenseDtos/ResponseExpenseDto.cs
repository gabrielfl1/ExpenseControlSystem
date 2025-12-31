using System.Text.Json.Serialization;

namespace ExpenseControlSystem.DTOs.ExpenseDtos {
    public class ResponseExpenseDto {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; }

        public Guid SubCategoryId { get; set; }
        public Guid UserId { get; set; }

    }
}
