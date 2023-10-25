using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Queries.GetProducts;


public record GetProductsQuery : IRequest<List<GetProductsQueryResponse>>;

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

public record GetProductsQueryResponse
{
    public int ProductId { get; set; }
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}

