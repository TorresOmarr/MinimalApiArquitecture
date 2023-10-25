
using Api.Extensions;
using Api.Infrastructure.Persistence;
using FluentValidation;
using MediatR;

namespace Api.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<IResult>
{
    public int ProductId { get; set; }
    public double Price { get; set; }
    public string? Description { get; set; }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, IResult>
{
    private readonly MyAppDbContext _context;
    private readonly IValidator<UpdateProductCommand> _validator;

    public UpdateProductHandler(MyAppDbContext context, IValidator<UpdateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var result = _validator.Validate(command);
        if (!result.IsValid)
        {
            return Results.ValidationProblem(result.GetValidationProblems());
        }

        var product = await _context.Products.FindAsync(command.ProductId);

        if (product is null)
        {
            return Results.NotFound();
        }

        product.UpdateInfo(command);

        await _context.SaveChangesAsync();

        return Results.Ok();


    }
}

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty();
        RuleFor(r => r.Description).NotEmpty();
        RuleFor(r => r.Price).NotEmpty().GreaterThan(0);
    }
}