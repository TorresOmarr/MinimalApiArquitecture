using Microsoft.AspNetCore.Identity;

namespace Api.Infrastructure.Persistence.SeedData;


public static class SeedAuth
{
    public static async Task SeedUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var testUser = await userManager.FindByNameAsync("test_user");
        if (testUser is null)
        {
            testUser = new IdentityUser
            {
                UserName = "test_user"
            };

            await userManager.CreateAsync(testUser, "Passw0rd.1234");
            await userManager.CreateAsync(new IdentityUser
            {
                UserName = "other_user"
            }, "Passw0rd.1234");
        }
    }

    public static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole is null)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin"
            });
            var testUser = await userManager.FindByNameAsync("test_user");
            await userManager.AddToRoleAsync(testUser!, "Admin");


        }
    }

}
