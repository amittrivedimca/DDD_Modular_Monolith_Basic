using Catalog.Domain.Events;
using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catalog.Domain.Entities
{
    public class Category : AggregateRoot<CategoryId>
    {
        public const int MaxNameLength = 50;

        public string Name { get; set; } = string.Empty;
        public Photo? Image { get; set; }

        public Category() { }

        private Category(CategoryId id, string name)
        {
            Id = id;
            SetName(name);            
        }

        public static Category CreateNew(string name)
        {
            return new Category(CategoryId.CreateUnique(), name);            
        }

        public void OnCategoryCreated()
        {
            this.RaiseDomainEvent(new CategoryDomainEvent(this.Id, this.Name, "CategoryCreated"));
        }

        public void OnCategoryUpdated()
        {
            this.RaiseDomainEvent(new CategoryDomainEvent(this.Id, this.Name, "CategoryUpdated"));
        }

        public static Category CreateWithId(Guid categoryId, string name)
        {
            return new Category(CategoryId.Create(categoryId), name);
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var normalized = name.Trim();
            if (normalized.Length > MaxNameLength)
                throw new ArgumentException($"Category name cannot exceed {MaxNameLength} characters.", nameof(name));

            Name = normalized;
        }

        public Category SetImage(string? imageName, string? imageUrl)
        {
            Image = imageName == null || imageUrl == null ? null : new Photo(imageName, imageUrl);
            return this;
        }
    }
}
