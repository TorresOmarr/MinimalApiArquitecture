using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Features.Auth.Command;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace Api.IntegrationTests;

public class TestBase
{
    protected ApiWebApplication Application;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTestsAsync()
    {
        Application = new ApiWebApplication();

        using var scope = Application.Services.CreateScope();

        await EnsureDatabaseAsync(scope);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        Application.Dispose();
    }


    [SetUp]
    public async Task Setup()
    {
        await ResetState();
    }

    [TearDown]
    public void Down()
    {

    }
    /// <summary>
    /// Crea un HttpClient incluyendo un JWT válido con usuario Admin
    /// </summary>
    public Task<(HttpClient Client, string UserId)> GetClientAsAdmin() =>
        CreateTestUser("user@admin.com", "Pass.W0rd", new string[] { "Admin" });

    /// <summary>
    /// Crea un HttpClient incluyendo un JWT válido con usuario default
    /// </summary>
    public Task<(HttpClient Client, string UserId)> GetClientAsDefaultUserAsync() =>
        CreateTestUser("user@normal.com", "Pass.W0rd", Array.Empty<string>());
    protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Crea un usuario de prueba según los parámetros
    /// </summary>
    /// <returns></returns>
    public async Task<(HttpClient Client, string UserId)> CreateTestUser(string userName, string password, string[] roles)
    {
        using var scope = Application.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var newUser = new IdentityUser(userName);

        await userManager.CreateAsync(newUser, password);

        foreach (var role in roles)
        {
            await userManager.AddToRoleAsync(newUser, role);
        }

        var accessToken = await GetAccessToken(userName, password);

        var client = Application.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return (client, newUser.Id);
    }

    /// <summary>
    /// Shortcut para autenticar un usuario para pruebas
    /// </summary>
    private async Task<string?> GetAccessToken(string userName, string password)
    {
        using var scope = Application.Services.CreateScope();
        var client = Application.CreateClient();
        var tokenCommand = new TokenCommand(userName, password);
        var response = await client.PostAsJsonAsync<TokenCommand>("/api/v1/auth/login", tokenCommand);
        TokenCommandResponse? tokenCommandResponse = await response.Content.ReadFromJsonAsync<TokenCommandResponse>();
        return tokenCommandResponse?.AccessToken;
    }

    protected async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = Application.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }
    private static async Task EnsureDatabaseAsync(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<MyAppDbContext>();
        context.Database.Migrate();
        await SeedRoles(scope);
    }
    private async Task ResetState()
    {
        var checkpoint = await Respawner.CreateAsync(ApiWebApplication.TestConnectionString, new RespawnerOptions
        {

            TablesToIgnore = new Table[]
            {
                "__EFMigrationsHistory",
                "AspNetRoles"
            },

        });
        await checkpoint.ResetAsync(ApiWebApplication.TestConnectionString);
    }

    public static async Task SeedRoles(IServiceScope scope)
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole is null)
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin"
            });

        }
    }
}