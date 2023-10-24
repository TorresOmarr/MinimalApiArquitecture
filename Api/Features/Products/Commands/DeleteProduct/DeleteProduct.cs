using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Products.Commands.DeleteProduct;


public class DeleteProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"/api/{nameof(Product)}/{{productId}}", async (int productId, IMediator mediator) =>
        {
            var command = new DeleteProductCommand(productId);

            return await mediator.Send(command);

        })
        .WithName(nameof(DeleteProduct))
        .WithTags(nameof(Product))
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization("admin_greetings")
        ;
    }
}

public class DeleteProductCommand : IRequest<IResult>
{
    public int ProductId { get; init; }
    public DeleteProductCommand(int productId)
    {
        ProductId = productId;
    }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IResult>
{
    private readonly MyAppDbContext _context;

    public DeleteProductCommandHandler(MyAppDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.ProductId);

        if (product is null)
        {
            return Results.NotFound();
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Results.Ok();
    }
}
