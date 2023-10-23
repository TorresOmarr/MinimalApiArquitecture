using Api.Domain.Entities;
using Domain;

namespace Api.Domain.Events;

public class ProductUpdateEvent : DomainEvent
{
    public ProductUpdateEvent(Product product)
    {
        Product = product;
    }
    public Product Product { get; set; } = default!;
}