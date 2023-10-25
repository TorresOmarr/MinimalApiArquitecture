using Api.Features.Auth;
using Api.Features.Products;

namespace Api.Features;


public static class RegisterEndpoints
{
    public static void RegisterEndpointsV1(this IEndpointRouteBuilder endpoints)
    {
        var endpointVersion = endpoints.MapGroup($"api/v1");

        ProductRoute.RegisterProductEndpoints(endpointVersion);
        AuthRoutes.RegisterAuthEndpoints(endpointVersion);
    }
}