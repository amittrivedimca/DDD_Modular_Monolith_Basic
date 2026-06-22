using System;
using System.Collections.Generic;
using System.Text;

namespace SharedKernel
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IHasDomainEvent where TId : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <summary>
        /// The collection of domain events raised by this aggregate.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Raises a domain event to be dispatched after the aggregate is persisted.
        /// </summary>
        /// <param name="domainEvent">The domain event to raise.</param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events from this aggregate.
        /// Called after events have been dispatched.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
