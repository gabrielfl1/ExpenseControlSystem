namespace ExpenseControlSystem.Models {
    public class SubCategory {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }


        // associação
        public Category Category{ get; set; }
        public List<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
