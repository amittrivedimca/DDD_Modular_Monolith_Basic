using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Entities
{
    public class Category : AggregateRoot<CategoryId>
    {
        public Category() { }

        public Category(CategoryId id) {
            Id = id;
        }
                
        public string Name { get; set; }
        public Photo? Image { get; set; }
    }
}
