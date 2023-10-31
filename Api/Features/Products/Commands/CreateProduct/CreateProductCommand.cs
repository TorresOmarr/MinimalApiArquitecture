using Api.Domain.Entities;
using Api.Extensions;
using Api.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Api.Features.Products.Commands.CreateProduct;

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


    public async Task<IResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        ValidationResult result = _validator.Validate(command);

        if (!result.IsValid)
        {
            return Results.ValidationProblem(result.GetValidationProblems());
        }

        var newProduct = Product.Create(productId: 0, description: command.Description, price: command.Price);

        _context.Products.Add(newProduct);

        await _context.SaveChangesAsync();

        return Results.Created($"/api/{nameof(Product)}/{newProduct.ProductId}", newProduct.ProductId);
    }
}

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(r => r.Description).NotNull().NotEmpty().MaximumLength(150);
        RuleFor(r => r.Price).NotNull().GreaterThan(0);
    }
}