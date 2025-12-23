using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControlSystem.Data.Mappings {
    public class ExpenseMap : IEntityTypeConfiguration<Expense> {
        public void Configure(EntityTypeBuilder<Expense> builder) {

            builder.ToTable("Expenses");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.PaidAt).IsRequired(false);
            builder.Property(x => x.IsPaid).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();

            builder
                .HasOne(x => x.SubCategory)
                .WithMany(x => x.Expenses)
                .HasForeignKey("SubCategoryId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(X => X.Expenses)
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
