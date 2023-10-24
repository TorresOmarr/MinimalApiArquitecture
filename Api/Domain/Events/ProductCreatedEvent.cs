using Api.Domain.Entities;
using Domain;

namespace Api.Domain.Events;

public class ProductCreatedEvent : DomainEvent
{
    public ProductCreatedEvent(Product product)
    {
        Product = product;
    }
    public Product Product { get; set; } = default!;
}