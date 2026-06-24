using Cart.Domain.ValueObjects;
using SharedKernel;
using System;

namespace Cart.Domain.Events
{
    public record CartCreatedDomainEvent(CartId CartId) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

    public record CartUpdatedDomainEvent(CartId CartId) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
