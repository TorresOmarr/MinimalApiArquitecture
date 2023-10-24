using Api.Domain.Events;
using Api.Features.Products.Commands.UpdateProduct;
using Domain;

namespace Api.Domain.Entities;

public class Product : BaseEntity, IHasDomainEvent
{
    public int ProductId { get; init; }
    public string Description { get; private set; } = default!;
    public double Price { get; private set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    public Product()
    {

    }
    public Product(int productId, string description, double price)
    {
        ProductId = productId;
        Description = description;
        Price = price;
        DomainEvents.Add(new ProductCreatedEvent(this));
    }
    public void UpdateInfo(UpdateProductCommand command)
    {
        Description = command.Description!;
        Price = command.Price;
        DomainEvents.Add(new ProductUpdateEvent(this));
    }
}