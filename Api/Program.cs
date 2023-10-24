using Api.Common.Interfaces;
using Api.Extensions;
using Api.Helpers;
using Api.Infrastructure.Persistence.SeedData;
using Api.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddCustomCors();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddEndpoints();
builder.Services.AddMediator();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddIdentity();
builder.Services.AddPolicies();
builder.Services.AddJWT(builder.Configuration);

var app = builder.Build();

app.UseCors(AppConstants.CorsPolicy);
app.MapSwagger();
await SeedData.InitializeDataAsync(app.Services);
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

app.Run();
