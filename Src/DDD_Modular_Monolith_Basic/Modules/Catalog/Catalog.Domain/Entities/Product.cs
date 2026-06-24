using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities
{
    public class Product : AggregateRoot<ProductId>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Photo? Image { get; set; }
        public CategoryId CategoryId { get; set; }
        public Category? Category { get; set; } = null;
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
