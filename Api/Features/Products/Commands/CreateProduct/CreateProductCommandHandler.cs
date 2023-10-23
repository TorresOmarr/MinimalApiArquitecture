using Api.Infrastructure.Persistence;
using Domain.Entities;
using MediatR;

namespace Api.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Unit>
{
    private readonly MyAppDbContext _context;

    public CreateProductCommandHandler(MyAppDbContext context)
    {
        _context = context;
    }


    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = new Product
        {
            Description = request.Description,
            Price = request.Price
        };

        _context.Products.Add(newProduct);

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}