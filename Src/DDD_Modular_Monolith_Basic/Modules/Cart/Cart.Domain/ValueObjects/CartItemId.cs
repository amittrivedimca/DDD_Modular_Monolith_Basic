using SharedKernel;
using System;
using System.Collections.Generic;

namespace Cart.Domain.ValueObjects
{
    public sealed class CartItemId : ValueObject
    {
        public Guid Id { get; }

        private CartItemId(Guid value)
        {
            Id = value;
        }

        public static CartItemId CreateUnique() => new(Guid.NewGuid());

        public static CartItemId Create(Guid? value)
        {
            if (value == null || value == Guid.Empty)
                throw new ArgumentException("CartItem ID cannot be empty.", nameof(value));

            return new CartItemId(value.Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public override string ToString() => Id.ToString();
    }
}
