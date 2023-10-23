using Api.Extensions;
using Api.Features.Products.Queries.GetProducts;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Queries.GetProduct;


public class GetProductByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"/api/{nameof(Product)}/{{productId}}", async (int productId, IMediator mediator) =>
        {
            var query = new GetProductByIdQuery
            {
                ProductId = productId
            };

            var queryResult = await mediator.Send(query);

            return Results.Ok(queryResult);
        });
    }
}