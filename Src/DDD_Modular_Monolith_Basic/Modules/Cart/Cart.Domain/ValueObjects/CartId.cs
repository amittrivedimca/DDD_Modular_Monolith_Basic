using SharedKernel;
using System;
using System.Collections.Generic;

namespace Cart.Domain.ValueObjects
{
    public sealed class CartId : ValueObject
    {
        public Guid Id { get; }

        private CartId(Guid value)
        {
            Id = value;
        }

        public static CartId CreateUnique() => new(Guid.NewGuid());

        public static CartId Create(Guid? value)
        {
            if (value == null || value == Guid.Empty)
                throw new ArgumentException("Cart ID cannot be empty.", nameof(value));

            return new CartId(value.Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString() => Id.ToString();
    }
}
