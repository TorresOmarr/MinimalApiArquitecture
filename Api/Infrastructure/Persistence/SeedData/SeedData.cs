
namespace Api.Infrastructure.Persistence.SeedData;

public class SeedData
{
    public static async Task InitializeDataAsync(IServiceProvider serviceProvider)
    {
        await SeedAuth.SeedUsers(serviceProvider);
        await SeedAuth.SeedRoles(serviceProvider);
        await SeddProduct.SeedProducts(serviceProvider);
    }
}