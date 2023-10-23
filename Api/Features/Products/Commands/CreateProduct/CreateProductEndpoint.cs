using Api.Extensions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Commands.CreateProduct;


public class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"/api/{nameof(Product)}", async ([FromBody] CreateProductCommand command, IMediator mediator) =>
       {
           var value = await mediator.Send(command);
           return Results.Ok(value);
       });
    }
}