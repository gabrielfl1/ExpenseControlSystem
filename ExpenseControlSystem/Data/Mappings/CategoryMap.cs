using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControlSystem.Data.Mappings {
    public class CategoryMap : IEntityTypeConfiguration<Category> {
        public void Configure(EntityTypeBuilder<Category> builder) {

            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.HasIndex(x => x.Name)
                .IsUnique();


            builder.HasData(
                new Category { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Alimentação", Description = "Despesa com alimentação"},
                new Category { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Transporte", Description = "Despesa com transporte" }
            );
        }
    }
}
