using MediatR;
using Api.Features.Auth.Command;

namespace Api.Features.Auth;


public static class AuthEndpoints
{
    public static void MapGroup(IEndpointRouteBuilder endpoints)
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