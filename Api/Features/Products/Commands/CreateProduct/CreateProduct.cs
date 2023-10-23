using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Commands.CreateProduct;
public class CreateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"/api/{nameof(Product)}", async ([FromBody] CreateProductCommand command, IMediator mediator) =>
       {
           return await mediator.Send(command);
       })
        .WithName(nameof(CreateProduct))
        .WithTags(nameof(Product))
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status201Created);
    }
}

public class CreateProductCommand : IRequest<IResult>
{
    public string Description { get; set; } = default!;
    public double Price { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, IResult>
{
    private readonly MyAppDbContext _context;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductCommandHandler(MyAppDbContext context, IValidator<CreateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }


    public async Task<IResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ValidationResult result = _validator.Validate(request);

        if (!result.IsValid)
        {
            return Results.ValidationProblem(result.GetValidationProblems());
        }

        var newProduct = new Product
        {
            Description = request.Description,
            Price = request.Price
        };

        _context.Products.Add(newProduct);

        await _context.SaveChangesAsync();

        return Results.Created($"/api/{nameof(Product)}/{newProduct.ProductId}", newProduct);
    }
}

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(r => r.Description).NotNull();
        RuleFor(r => r.Price).NotNull().GreaterThan(0);
    }
}