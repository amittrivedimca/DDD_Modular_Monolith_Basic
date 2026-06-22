using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Infrastructure.Persistance.Interceptors
{
    /// <summary>
    /// EF Core interceptor that dispatches domain events after saving changes.
    /// This ensures events are only published after the transaction is committed.
    /// </summary>
    public sealed class DomainEventDispatcherInterceptor : SaveChangesInterceptor
    {
        private readonly IPublisher _publisher;

        public DomainEventDispatcherInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override async ValueTask<int> SavedChangesAsync(
       SaveChangesCompletedEventData eventData,
       int result,
       CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken cancellationToken)
        {
            // Get all aggregates that have domain events
            var aggregatesWithEvents = context.ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .OfType<IHasDomainEvent>()
                .Where(e => e.DomainEvents.Any())
                .ToList();

            // Collect all domain events
            var domainEvents = aggregatesWithEvents
                .SelectMany(a => a.DomainEvents)
                .ToList();

            // Clear events from aggregates
            aggregatesWithEvents.ForEach(a => a.ClearDomainEvents());

            // Publish each event
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
