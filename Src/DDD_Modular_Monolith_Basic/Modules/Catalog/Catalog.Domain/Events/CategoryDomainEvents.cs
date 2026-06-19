using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Events
{

    public sealed record CategoryCreatedDomainEvent(CategoryId CategoryId, string Name) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
