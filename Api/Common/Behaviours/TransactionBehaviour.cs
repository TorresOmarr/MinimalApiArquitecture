using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
{
    private readonly MyAppDbContext _context;
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

    public TransactionBehaviour(MyAppDbContext context, ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await _context.BeginTransactionAsync();
            var response = await next();
            await _context.CommitTransactionAsync();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("Request failed: Rolling back all the changes made to the Context {@Error}", ex.Message);

            await _context.RollbackTransaction();
            throw;
        }
    }
}