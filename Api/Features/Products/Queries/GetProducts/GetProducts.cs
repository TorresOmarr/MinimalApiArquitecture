using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Queries.GetProducts;


public class GetProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"/api/{nameof(Product)}", async (IMediator mediator) =>
        {
            var query = new GetProductsQuery();
            return await mediator.Send(query);
        })
        .WithName(nameof(GetProducts))
        .WithTags(nameof(Product))
        .Produces<List<GetProductsQueryResponse>>();

    }
}

public class GetProductsQuery : IRequest<List<GetProductsQueryResponse>>
{
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsQueryResponse>>
{
    private readonly MyAppDbContext _context;

    public GetProductsQueryHandler(MyAppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {

        var response = _context.Products
             .AsNoTracking()
             .Select(s => new GetProductsQueryResponse
             {
                 ProductId = s.ProductId,
                 Description = s.Description,
                 Price = s.Price
             })
             .ToListAsync();
        return response;
    }
}

public class GetProductsQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}