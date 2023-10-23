using Api.Domain.Events;
using MediatR;

namespace Api.Features.Products.EventHandlers;

public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdateEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger;

    public ProductUpdatedEventHandler(ILogger<ProductUpdatedEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(ProductUpdateEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Minimal APIs Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
