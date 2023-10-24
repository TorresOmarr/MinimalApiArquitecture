using Api.Domain.Events;
using MediatR;

namespace Api.Features.Products.EventHandlers;

public class ProductCreateddEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreateddEventHandler> _logger;

    public ProductCreateddEventHandler(ILogger<ProductCreateddEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Minimal APIs Domain Event: {DomainEvent}", notification.GetType().Name);
        _logger.LogWarning("Minimal APIs Domain Event: {DomainEvent}", notification.Product.Description);

        return Task.CompletedTask;
    }
}
