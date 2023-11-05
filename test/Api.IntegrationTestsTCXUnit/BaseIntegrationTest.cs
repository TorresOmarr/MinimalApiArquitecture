using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.Features.Auth.Command;
using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTestsTCXUnit;



public abstract class BaseIntegrationTest : IClassFixture<IntergrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly HttpClient _client;
    protected readonly MyAppDbContext _context;
    protected BaseIntegrationTest(IntergrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _client = factory.CreateClient();
        _context = _scope.ServiceProvider.GetRequiredService<MyAppDbContext>();
    }


    public Task<(HttpClient Client, string UserId)> GetClientAsAdmin() =>
        CreateTestUser("user@admin.com", "Pass.W0rd", new string[] { "Admin" });

    public Task<(HttpClient Client, string UserId)> GetClientAsDefaultUserAsync() =>
        CreateTestUser("user@normal.com", "Pass.W0rd", Array.Empty<string>());

    public async Task<(HttpClient Client, string UserId)> CreateTestUser(string userName, string password, string[] roles)
    {
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var newUser = new IdentityUser(userName);

        await userManager.CreateAsync(newUser, password);

        foreach (var role in roles)
        {
            await userManager.AddToRoleAsync(newUser, role);
        }

        var accessToken = await GetAccessToken(userName, password);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return (_client, newUser.Id);
    }
    private async Task<string?> GetAccessToken(string userName, string password)
    {
        var tokenCommand = new TokenCommand(userName, password);
        var response = await _client.PostAsJsonAsync<TokenCommand>("/api/v1/auth/login", tokenCommand);
        TokenCommandResponse? tokenCommandResponse = await response.Content.ReadFromJsonAsync<TokenCommandResponse>();
        return tokenCommandResponse?.AccessToken;
    }

    protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
    {

        _context.Add(entity);

        await _context.SaveChangesAsync();

        return entity;
    }

}