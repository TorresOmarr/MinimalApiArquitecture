using Api.Extensions;
using Api.Infrastructure.Persistence;
using Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddSqlite<MyAppDbContext>(builder.Configuration.GetConnectionString("Default"));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");
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
