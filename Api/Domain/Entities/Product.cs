using Api.Domain.Events;
using Api.Features.Products.Commands.UpdateProduct;
using Domain;

namespace Api.Domain.Entities;

public class Product : IHasDomainEvent
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    public void UpdateInfo(UpdateProductCommand command)
    {
        Description = command.Description!;
        Price = command.Price;
        DomainEvents.Add(new ProductUpdateEvent(this));
    }
}