using Api.Domain.Entities;
using Api.Features.Products.Commands.CreateProduct;
using Api.Features.Products.Commands.DeleteProduct;
using Api.Features.Products.Commands.UpdateProduct;
using Api.Features.Products.Queries.GetProduct;
using Api.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products;


public static class ProductRoute
{
    public static void RegisterProductEndpoints(IEndpointRouteBuilder endpoints)
    {
        var productsGroup = endpoints.MapGroup($"/{nameof(Product)}")
                            .WithTags(nameof(Product))
                            .RequireAuthorization("admin_greetings");


        productsGroup.MapGet("", async (IMediator mediator) =>
        {
            var query = new GetProductsQuery();
            return await mediator.Send(query);
        })
        .Produces<List<GetProductsQueryResponse>>();



        productsGroup.MapGet("/{productId}", async (int productId, IMediator mediator) =>
        {
            var query = new GetProductByIdQuery(productId);

            return await mediator.Send(query);

        })
        .Produces<GetProductByIdQueryResponse>();

        productsGroup.MapPost("", async ([FromBody] CreateProductCommand command, IMediator mediator) =>
              {
                  return await mediator.Send(command);
              })
               .ProducesValidationProblem()
               .Produces(StatusCodes.Status201Created)
               .Produces(StatusCodes.Status404NotFound);

        productsGroup.MapDelete("/{productId}", async (int productId, IMediator mediator) =>
               {
                   var command = new DeleteProductCommand(productId);

                   return await mediator.Send(command);

               })
               .Produces(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status404NotFound);

        productsGroup.MapPut("/{productId}", async (int productId, IMediator mediator, UpdateProductCommand command) =>
                {
                    command.ProductId = productId;
                    await mediator.Send(command);

                })
                .Produces(StatusCodes.Status404NotFound)
                .ProducesValidationProblem();
    }

}