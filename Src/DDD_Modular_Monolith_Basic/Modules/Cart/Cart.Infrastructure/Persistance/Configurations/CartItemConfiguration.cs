using Cart.Domain.Entities;
using Cart.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistance.Configurations
{
    public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Id,
                    value => CartItemId.Create(value))
                .ValueGeneratedNever()
                .HasColumnName("Id");

            builder.Property(x => x.CartId)
                .HasConversion(
                    id => id.Id,
                    value => CartId.Create(value))
                .IsRequired()
                .HasColumnName("CartId");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Name");

            builder.Property(x => x.ImageName)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName("ImageName");

            builder.Property(x => x.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName("ImageUrl");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(13, 2)
                .HasColumnName("Price");

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnName("Quantity");
        }
    }
}
