using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControlSystem.Data.Mappings {
    public class SubCategoryMap : IEntityTypeConfiguration<SubCategory> {
        public void Configure(EntityTypeBuilder<SubCategory> builder) {
            builder.ToTable("SubCategories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new SubCategory {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 
                    Name = "Ifood", 
                    Description = "Despesas de alimentação por delivery", 
                    CategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                },
                new SubCategory {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Restaurante",
                    Description = "Despesas com saidas ao restaurante",
                    CategoryId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                },

                new SubCategory {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Uber",
                    Description = "Despesas com transporte via aplicativo",
                    CategoryId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                },
                new SubCategory {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Name = "Combustível",
                    Description = "Despesas com abastecimento de veículos",
                    CategoryId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                }
            );
        }
    }
}
