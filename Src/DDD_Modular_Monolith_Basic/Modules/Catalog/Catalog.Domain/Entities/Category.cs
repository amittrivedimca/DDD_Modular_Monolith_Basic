using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities
{
    public class Category : AggregateRoot<CategoryId>
    {
        public CategoryId Value { get; set; }
        public string Name { get; set; }
        public Photo Image { get; set; }
    }
}
