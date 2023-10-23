using Api.Extensions;
using Domain.Entities;
using MediatR;

namespace Api.Features.Products.Queries.GetProducts;


public class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"/api/{nameof(Product)}s", async (IMediator mediator) =>
        {
            var query = new GetProductsQuery();
            var products = await mediator.Send(query);
            return Results.Ok(products);
        });
    }
}