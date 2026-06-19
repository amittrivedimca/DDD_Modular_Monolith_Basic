using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Persistance.Configurations
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            // Configure the Category entity here
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Id)
            .HasConversion(
                id => id.Id,
                value => CategoryId.Create(value))
            .ValueGeneratedNever()
            .HasColumnName("Id");

            builder.OwnsOne(x => x.Image, photo =>
            {
                photo.Property(p => p.Name).HasColumnName("ImageName");
                photo.Property(p => p.Url).HasColumnName("ImageUrl");
            });
        }
    }
}
