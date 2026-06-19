using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.ValueObjects
{
    public class CategoryId : ValueObject
    {
        public Guid Id { get; }

        private CategoryId(Guid value)
        {
            Id = value;
        }

        public static CategoryId CreateUnique() => new(Guid.NewGuid());

        public static CategoryId Create(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Customer ID cannot be empty.", nameof(value));

            return new CategoryId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString() => Id.ToString();
    }
}
