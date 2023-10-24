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
            new Product(0,"Product 01",16000),
            new Product(0,"Product 02",26000)
        });

            await context.SaveChangesAsync();
        }
    }
}