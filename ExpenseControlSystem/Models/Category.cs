namespace ExpenseControlSystem.Models {
    public class Category {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string? Description { get; set; }


        //assocíação
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
