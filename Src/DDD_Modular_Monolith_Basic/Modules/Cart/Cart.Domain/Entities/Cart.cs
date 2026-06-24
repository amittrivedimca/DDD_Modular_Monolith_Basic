using Cart.Domain.Events;
using Cart.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cart.Domain.Entities
{
    public sealed class Cart : AggregateRoot<CartId>
    {
        private readonly List<CartItem> _cartItems = [];

        public Guid? UserId { get; private set; }
        public DateTime? CreatedDate { get; private set; }
        public string? Status { get; private set; }
        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        // Required by EF Core
        private Cart() { }

        private Cart(CartId id)
        {
            Id = id;
        }

        public static Cart CreateNew() =>
            new(CartId.CreateUnique());

        public static Cart CreateWithId(Guid id) =>
            new(CartId.Create(id));

        public void OnCreated() => RaiseDomainEvent(new CartCreatedDomainEvent(Id));
        public void OnUpdated() => RaiseDomainEvent(new CartUpdatedDomainEvent(Id));

        public CartItem AddItem(string name, decimal price, int quantity)
        {
            var existing = _cartItems.FirstOrDefault(i => i.Name == name);
            if (existing is not null)
            {
                existing.UpdateQuantity(existing.Quantity + quantity);
                return existing;
            }

            var item = CartItem.CreateNew(Id, name, price, quantity);
            _cartItems.Add(item);
            return item;
        }

        public void RemoveItem(Guid cartItemId)
        {
            var item = _cartItems.FirstOrDefault(i => i.Id.Id == cartItemId);
            if (item is not null)
                _cartItems.Remove(item);
        }

        public Cart SetUserId(Guid? userId)
        {
            UserId = userId;
            return this;
        }

        public Cart SetCreatedDate(DateTime? createdDate)
        {
            CreatedDate = createdDate;
            return this;
        }

        public Cart SetStatus(string? status)
        {
            Status = status;
            return this;
        }
    }
}
