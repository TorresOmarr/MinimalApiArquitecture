using Api.Common.Interfaces;
using Api.Domain;
using Api.Domain.Entities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Infrastructure.Persistence;

public class MyAppDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IPublisher _publisher;
    private readonly ILogger<MyAppDbContext> _logger;
    private IDbContextTransaction? _currentTransaction;
    private readonly CurrentUser _user;

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options, IPublisher publisher, ILogger<MyAppDbContext> logger, ICurrentUserService currentUserService) : base(options)
    {
        _publisher = publisher;
        _logger = logger;

        _logger.LogDebug("DbContext created.");
        _user = currentUserService.User;
        Database.Migrate();
    }

    public DbSet<Product> Products => Set<Product>();


    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            _logger.LogInformation("A transaction with ID {ID} is already created", _currentTransaction.TransactionId);
            return;
        }


        _currentTransaction = await Database.BeginTransactionAsync();
        _logger.LogInformation("A new transaction was created with ID {ID}", _currentTransaction.TransactionId);
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        _logger.LogInformation("Commiting Transaction {ID}", _currentTransaction.TransactionId);

        await _currentTransaction.CommitAsync();

        _currentTransaction.Dispose();
        _currentTransaction = null;
    }

    public async Task RollbackTransaction()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        _logger.LogDebug("Rolling back Transaction {ID}", _currentTransaction.TransactionId);

        await _currentTransaction.RollbackAsync();

        _currentTransaction.Dispose();
        _currentTransaction = null;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _user.Id;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _user.Id;
                    entry.Entity.LastModifiedByAt = DateTime.UtcNow;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        foreach (var @event in events)
        {
            @event.IsPublished = true;

            _logger.LogInformation("New domain event {Event}", @event.GetType().Name);

            // Note: If an unhandled exception occurs, all the saved changes will be rolled back
            // by the TransactionBehavior. All the operations related to a domain event finish
            // successfully or none of them do.
            // Reference: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation#what-is-a-domain-event
            await _publisher.Publish(@event);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);
    }
}