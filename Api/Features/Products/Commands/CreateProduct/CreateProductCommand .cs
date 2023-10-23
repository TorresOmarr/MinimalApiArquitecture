using MediatR;

namespace Api.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Unit>
{
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}


