using ExpenseControlSystem.Data.Mappings;
using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControlSystem.Data {
    public class ExpenseControlSystemDataContext : DbContext {

        public ExpenseControlSystemDataContext(DbContextOptions<ExpenseControlSystemDataContext> options): base(options) {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new ExpenseMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new SubCategoryMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}

