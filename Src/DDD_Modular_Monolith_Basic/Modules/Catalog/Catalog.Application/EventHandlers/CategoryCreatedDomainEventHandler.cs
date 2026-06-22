using Catalog.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.EventHandlers
{
    public sealed class CategoryCreatedDomainEventHandler : INotificationHandler<CategoryDomainEvent>
    {
        private readonly ILogger<CategoryCreatedDomainEventHandler> _logger;

        public CategoryCreatedDomainEventHandler(ILogger<CategoryCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CategoryDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Category {CategoryId} created with name {CategoryName} at {OccurredOn}",
                notification.CategoryId.Id,
                notification.Name,
                notification.OccurredOn);

            // Here you could:
            // - Send a notification to other service            
            // - Trigger other workflows

            return Task.CompletedTask;
        }
    }
}
