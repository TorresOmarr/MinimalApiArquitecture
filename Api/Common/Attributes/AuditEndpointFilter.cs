
using Api.Common.Interfaces;

namespace Api.Common.Attributes;

public class AuditEndpointFilter : IEndpointFilter
{
    private readonly ILogger<AuditEndpointFilter> _logger;
    private readonly ICurrentUserService _currentUserService;

    public AuditEndpointFilter(ILogger<AuditEndpointFilter> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        _logger.LogInformation("User {@User} with requestPath {@RequestPath} and method {@Method}", _currentUserService.User, context.HttpContext.Request.Path.Value, context.HttpContext.Request.Method);

        return next(context);

    }
}