using Api.Domain.Entities;
using Api.Extensions;
using Api.Helpers;
using Api.Infrastructure.Persistence;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomCors();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddEndpoints();
builder.Services.AddMediator();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));



var app = builder.Build();

app.UseCors(AppConstants.CorsPolicy);
app.MapSwagger();
app.UseStaticFiles();
await SeedProducts();


app.MapEndpoints();
async Task SeedProducts()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();

    if (!context.Products.Any())
    {
        context.Products.AddRange(new List<Product>
        {
            new Product
            {
                Description = "Product 01",
                Price = 16000
            },
            new Product
            {
                Description = "Product 02",
                Price = 52200
            }
        });

        await context.SaveChangesAsync();
    }
}
app.Run();
