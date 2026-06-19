using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistance.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Id,
                    value => ProductId.Create(value))
                .ValueGeneratedNever()
                .HasColumnName("Id");

            builder.Property(x => x.CategoryId)
                .HasConversion(
                    id => id.Id,
                    value => CategoryId.Create(value))
                .HasColumnName("CategoryId");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .HasColumnName("Name");

            builder.Property(x => x.Description)
                .HasMaxLength(50)
                .HasColumnName ("Description");

            builder.OwnsOne(x => x.Image, photo =>
            {
                photo.Property(p => p.Name).HasColumnName("ImageName");
                photo.Property(p => p.Url).HasColumnName("ImageUrl");
            });
        }
    }
}
