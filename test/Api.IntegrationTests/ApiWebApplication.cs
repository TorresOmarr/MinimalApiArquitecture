using Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api.IntegrationTests;

public class ApiWebApplication : WebApplicationFactory<Api>
{
    public const string TestConnectionString = "Server=localhost,1434;Database=VSA_TestDb;User Id=sa;Password=PasswordO1.;TrustServerCertificate=True";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                return new DbContextOptionsBuilder<MyAppDbContext>()
                .UseSqlServer(TestConnectionString)
                .UseApplicationServiceProvider(sp)
                .Options;
            });
        });
        return base.CreateHost(builder);
    }
}