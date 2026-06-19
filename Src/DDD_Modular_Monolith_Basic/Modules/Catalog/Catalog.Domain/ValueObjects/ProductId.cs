using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.ValueObjects
{
    public class ProductId : ValueObject
    {
        public Guid Id { get; }

        private ProductId(Guid value)
        {
            Id = value;
        }

        public static ProductId CreateUnique() => new(Guid.NewGuid());

        public static ProductId Create(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("Customer ID cannot be empty.", nameof(value));

            return new ProductId(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString() => Id.ToString();
    }
}
