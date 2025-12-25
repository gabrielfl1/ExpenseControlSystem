namespace ExpenseControlSystem.Models {
    public class Expense {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; }

        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // associação
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
