using MediatR;
using Api.Features.Auth.Command;

namespace Api.Features.Auth;


public static class AuthRoutes
{
    public static void RegisterAuthEndpoints(IEndpointRouteBuilder endpoints)
    {
        var authGroup = endpoints.MapGroup($"/{nameof(Auth)}")
                            .WithTags(nameof(Auth));

        authGroup.MapPost("/login", async (TokenCommand command, IMediator mediator) =>
        {
            return await mediator.Send(command);
        })
        .Produces<TokenCommandResponse>();
    }
}