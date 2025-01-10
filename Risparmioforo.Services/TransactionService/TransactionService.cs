using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;
using Risparmioforo.Shared.Extensions;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Services.TransactionService;

public class TransactionService(
    ApplicationDbContext dbContext,
    ILogger<TransactionService> logger) 
    : ITransactionService
{
    public async Task<Result<Pagination<Transaction>>> Search(SearchCommand command, CancellationToken cancellationToken)
    {
        var query = dbContext.Transactions
            .AsNoTracking()
            .Where(string.IsNullOrEmpty(command.Query)
                ? transaction => true
                : transaction => transaction.Description.ToLower() == command.Query.ToLower())
            .AsQueryable();

        var totalItemsCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Include(x => x.Merchant)
            .Include(x => x.Items)
            .Skip(command.PageSize * command.PageIndex)
            .Take(command.PageSize)
            .ToListAsync(cancellationToken);

        var paginatedResults = items.ToPagination(command.PageIndex, command.PageSize, totalItemsCount);

        return Result<Pagination<Transaction>>.Success(paginatedResults);
    }

    public async Task<Result<Transaction>> Create(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var entity = command.ToEntity();
        
        dbContext.Transactions.Add(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Transaction created successfully.");
        
        return Result<Transaction>.Success(entity);
    }
    
    public async Task<Result<Transaction>> Update(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<Transaction>.Failure(TransactionErrors.NotFound(command.Id));
        
        entity = command.ToEntity(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Transaction updated successfully.");
        
        return Result<Transaction>.Success(entity);
    }
    
    public async Task<Result<bool>> Remove(RemoveTransactionCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<bool>.Failure(TransactionErrors.NotFound(command.Id));
        
        dbContext.Transactions.Remove(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Transaction removed successfully.");

        return Result<bool>.Success(true);
    }
}