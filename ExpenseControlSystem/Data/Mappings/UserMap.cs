using ExpenseControlSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Xml.Linq;

namespace ExpenseControlSystem.Data.Mappings {
    public class UserMap : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {

            builder.ToTable("Users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasData(
                new User { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "User", Email = "teste@gmail.com", CreatedAt = new DateTime(2025,12,25) }
            );
        }
    }
}
