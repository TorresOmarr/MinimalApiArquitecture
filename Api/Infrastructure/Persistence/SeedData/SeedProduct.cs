using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.SeedData;

public static class SeddProduct
{
    public static async Task SeedProducts(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
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
}