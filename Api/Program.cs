using Api.Common.Interfaces;
using Api.Extensions;
using Api.Features;
using Api.Helpers;
using Api.Infrastructure.Persistence.SeedData;
using Api.Services;
using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();


builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCustomCors();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddMediator();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddIdentity();
builder.Services.AddPolicies();
builder.Services.AddJWT(builder.Configuration);

var app = builder.Build();

app.RegisterEndpointsV1();


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.ApplicationInsights(app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
    .CreateLogger();

app.UseCors(AppConstants.CorsPolicy);
app.MapSwagger();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

try
{
    Log.Information("Iniciando Web API");

    await SeedData.InitializeDataAsync(app.Services);


    Log.Information("Corriendo en:");
    Log.Information("https://localhost:7299");
    Log.Information("http://localhost:5232");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");

    return;
}
finally
{
    Log.CloseAndFlush();
}
