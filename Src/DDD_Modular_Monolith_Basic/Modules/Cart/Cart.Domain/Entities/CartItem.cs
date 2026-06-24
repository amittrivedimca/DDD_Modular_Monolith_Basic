using Cart.Domain.ValueObjects;
using SharedKernel;
using System;

namespace Cart.Domain.Entities
{
    public sealed class CartItem : Entity<CartItemId>
    {
        public const int MaxNameLength = 30;

        public CartId CartId { get; private set; } = null!;
        public string Name { get; private set; } = string.Empty;
        public string? ImageName { get; private set; }
        public string? ImageUrl { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        // Required by EF Core
        private CartItem() { }

        private CartItem(CartItemId id, CartId cartId, string name, decimal price, int quantity)
        {
            Id = id;
            SetCartId(cartId);
            SetName(name);
            SetPrice(price);
            SetQuantity(quantity);
        }

        public static CartItem CreateNew(CartId cartId, string name, decimal price, int quantity) =>
            new(CartItemId.CreateUnique(), cartId, name, price, quantity);

        public static CartItem CreateWithId(Guid id, CartId cartId, string name, decimal price, int quantity) =>
            new(CartItemId.Create(id), cartId, name, price, quantity);

        public CartItem SetImageName(string? imageName)
        {
            ImageName = imageName;
            return this;
        }

        public CartItem SetImageUrl(string? imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        public CartItem UpdateQuantity(int quantity)
        {
            SetQuantity(quantity);
            return this;
        }

        public CartItem UpdatePrice(decimal price)
        {
            SetPrice(price);
            return this;
        }

        private void SetCartId(CartId cartId)
        {
            ArgumentNullException.ThrowIfNull(cartId);
            CartId = cartId;
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var normalized = name.Trim();
            if (normalized.Length > MaxNameLength)
                throw new ArgumentException($"CartItem name cannot exceed {MaxNameLength} characters.", nameof(name));

            Name = normalized;
        }

        private void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("Price cannot be negative.", nameof(price));

            Price = price;
        }

        private void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Quantity = quantity;
        }
    }
}
