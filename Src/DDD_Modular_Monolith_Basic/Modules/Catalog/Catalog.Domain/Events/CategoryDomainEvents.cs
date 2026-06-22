using Catalog.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Events
{



    public record CategoryDomainEvent(CategoryId CategoryId, string Name, string EventName) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public string EventName { get; } = EventName;
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        
    }
}
