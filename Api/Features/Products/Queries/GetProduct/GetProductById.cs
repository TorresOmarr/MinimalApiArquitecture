using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Products.Queries.GetProduct;


public class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"/api/{nameof(Product)}/{{productId}}", async (int productId, IMediator mediator) =>
        {
            var query = new GetProductByIdQuery
            {
                ProductId = productId
            };

            return await mediator.Send(query);

        })
        .WithName(nameof(GetProductById))
        .WithTags(nameof(Product))
        .Produces<GetProductByIdQueryResponse>()
        .RequireAuthorization("admin_greetings");
    }
}

public class GetProductByIdQuery : IRequest<IResult>
{
    public int ProductId { get; set; }
}

public class GetProductQueryHandler : IRequestHandler<GetProductByIdQuery, IResult>
{
    private readonly MyAppDbContext _context;

    public GetProductQueryHandler(MyAppDbContext context)
    {
        _context = context;
    }
    public async Task<IResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);
        if (product is null)
        {
            return Results.NotFound();
        }


        var productResponse = new GetProductByIdQueryResponse
        {
            Description = product.Description,
            ProductId = product.ProductId,
            Price = product.Price
        };

        return Results.Ok(productResponse);
    }
}

public class GetProductByIdQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}