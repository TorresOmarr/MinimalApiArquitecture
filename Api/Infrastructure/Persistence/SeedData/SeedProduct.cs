using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.SeedData;

public static class SeddProduct
{
    public static async Task SeedProducts(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();

        // if (!context.Products.Any())
        // {
        //     context.Products.AddRange(new List<Product>
        // {
        //     Product.Create(productId: 0, description: "Product 01", price: 25000),
        //     Product.Create(productId: 0, description: "Product 02", price: 35000)
        // });

        //     await context.SaveChangesAsync();
        // }
    }
}