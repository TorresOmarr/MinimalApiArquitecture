using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Features.Products.Queries.GetProduct;


public record GetProductByIdQuery(int ProductId) : IRequest<Results<Ok<GetProductByIdQueryResponse>, NotFound>>;

public class GetProductQueryHandler : IRequestHandler<GetProductByIdQuery, Results<Ok<GetProductByIdQueryResponse>, NotFound>>
{
    private readonly MyAppDbContext _context;

    public GetProductQueryHandler(MyAppDbContext context)
    {
        _context = context;
    }
    public async Task<Results<Ok<GetProductByIdQueryResponse>, NotFound>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product is null)
        {
            return TypedResults.NotFound();
        }


        var productResponse = new GetProductByIdQueryResponse
        {
            Description = product.Description,
            ProductId = product.ProductId,
            Price = product.Price
        };

        return TypedResults.Ok(productResponse);
    }
}

public class GetProductByIdQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}
