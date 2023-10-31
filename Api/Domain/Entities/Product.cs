using System.Runtime.InteropServices;
using Api.Domain.Events;
using Api.Features.Products.Commands.UpdateProduct;
using Domain;

namespace Api.Domain.Entities;

public class Product : BaseEntity, IHasDomainEvent
{
    public int ProductId { get; private set; }
    public string Description { get; private set; } = default!;
    public double Price { get; private set; }
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    public Product()
    {

    }
    private Product(int productId, string description, double price)
    {
        ProductId = productId;
        Description = description;
        Price = price;
        DomainEvents.Add(new ProductCreatedEvent(this));
    }

    public static Product Create(int productId, string description, double price)
    {
        if (productId != 0)
        {
            throw new ArgumentException("ProductId must be 0", nameof(productId));
        }
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be less than 0", nameof(price));
        }
        return new Product(productId, description, price);
    }
    public void UpdateInfo(UpdateProductCommand command)
    {
        Description = command.Description!;
        Price = command.Price;
        DomainEvents.Add(new ProductUpdateEvent(this));
    }
}