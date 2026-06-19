using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    /// <summary>
    /// Represents a domain event that occurred within an aggregate.
    /// Domain events are used to communicate between aggregates and modules.
    /// </summary>
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Unique identifier for this event instance.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The date and time when this event occurred.
        /// </summary>
        DateTime OccurredOn { get; }
    }

}
