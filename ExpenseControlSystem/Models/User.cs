namespace ExpenseControlSystem.Models {
    public class User {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //associação

        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
