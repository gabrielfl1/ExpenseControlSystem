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
                .HasForeignKey(x => x.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.User)
                .WithMany(X => X.Expenses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Expense {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Description = "Compra de lanche pelo Ifood",
                    Amount = 50.0m,
                    DueDate = new DateTime(2025, 12, 25),
                    IsPaid = true,
                    CreatedAt = new DateTime(2025, 12, 25),
                    SubCategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                },

                new Expense {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Description = "Ida ao trabalho",
                    Amount = 28.50m,
                    DueDate = new DateTime(2025, 12, 20),
                    IsPaid = true,
                    CreatedAt = new DateTime(2025, 12, 20),
                    SubCategoryId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                },

                new Expense {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Description = "viagem de uber para a praia",
                    Amount = 130.0m,
                    DueDate = new DateTime(2025, 12, 27),
                    IsPaid = false,
                    CreatedAt = new DateTime(2025, 12, 20),
                    SubCategoryId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    UserId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                }
            );
        }
    }
}
