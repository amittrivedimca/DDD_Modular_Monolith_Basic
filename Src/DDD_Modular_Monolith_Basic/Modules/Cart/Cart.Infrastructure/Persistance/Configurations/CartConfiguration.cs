using Cart.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistance.Configurations
{
    public sealed class CartConfiguration : IEntityTypeConfiguration<Domain.Entities.Cart>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Id,
                    value => CartId.Create(value))
                .ValueGeneratedNever()
                .HasColumnName("Id");

            builder.Property(x => x.UserId)
                .IsRequired(false)
                .HasColumnName("UserId");

            builder.Property(x => x.CreatedDate)
                .IsRequired(false)
                .HasColumnName("CreatedDate");

            builder.Property(x => x.Status)
                .IsRequired(false)
                .HasMaxLength(20)
                .HasColumnName("Status");

            // Configure one-to-many relationship with CartItems
            builder.HasMany(x => x.CartItems)
                .WithOne()
                .HasForeignKey("CartId")
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore domain events (transient, not persisted)
            builder.Ignore(x => x.DomainEvents);
        }
    }
}
