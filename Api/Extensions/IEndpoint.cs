namespace Api.Extensions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder endpoints);
}