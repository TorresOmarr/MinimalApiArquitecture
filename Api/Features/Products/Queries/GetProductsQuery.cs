using Api.Extensions;
using Domain.Entities;

namespace MinimalApiScrutor.Features.Products.Queries;

public class GetProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"/api/{nameof(Product)}", () => new List<Product>
        {
            new Product { ProductId = 1,  Description = "Description 1" },
            new Product { ProductId = 2, Description = "Description 2"},
            new Product { ProductId = 3,  Description = "Description 3" },
            new Product { ProductId = 4, Description = "Description 4"},
            new Product { ProductId = 5,  Description = "Description 5"},
        });
    }
}