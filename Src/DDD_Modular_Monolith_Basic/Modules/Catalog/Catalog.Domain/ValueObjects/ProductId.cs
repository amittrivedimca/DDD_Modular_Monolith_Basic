using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.ValueObjects
{
    public class ProductId : ValueObject
    {
        public Guid Value { get; }

        private ProductId(Guid value)
        {
            Value = value;
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
            yield return Value;
        }

        public override string ToString() => Value.ToString();
    }
}
