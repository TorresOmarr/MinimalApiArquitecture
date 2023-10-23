using Api.Exceptions;
using Api.Features.Products.Queries.GetProduct;
using Api.Infrastructure.Persistence;
using Domain.Entities;
using MediatR;
namespace Api.Features.Products.Queries.GetProduct;


public class GetProductQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
{
    private readonly MyAppDbContext _context;

    public GetProductQueryHandler(MyAppDbContext context)
    {
        _context = context;
    }
    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product is null)
        {
            throw new NotFoundException(nameof(Product), request.ProductId);

        }


        return new GetProductByIdQueryResponse
        {
            Description = product.Description,
            ProductId = product.ProductId,
            Price = product.Price
        };
    }
}
