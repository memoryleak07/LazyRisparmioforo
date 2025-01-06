using Microsoft.EntityFrameworkCore;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Extensions;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Api.Services.TransactionService;

public class TransactionService(
    ApplicationDbContext dbContext,
    ILogger<TransactionService> logger) 
    : ITransactionService
{
    public async Task<Result<Pagination<Transaction>>> Search(
        string? query, 
        int pageIndex = 0, 
        int pageSize = 10)
    {
        var transactions = await dbContext.Transactions
            .AsNoTracking()
            .Where(string.IsNullOrEmpty(query)
                ? transaction => true
                : transaction => transaction.Description.ToLower() == query.ToLower())
            .Take(pageSize)
            .Skip(pageIndex * pageSize)
            .ToListAsync();

        return Result<Pagination<Transaction>>.Success(transactions.ToPagination(pageIndex, pageSize));
    }

    public async Task<Result<Transaction>> Create(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity();
        
        dbContext.Transactions.Add(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<Transaction>.Success(entity);
    }
    
    public async Task<Result<Transaction>> Update(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<Transaction>.Failure(TransactionErrors.NotFound(request.Id));
        
        entity = request.ToEntity(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<Transaction>.Success(entity);
    }
    
    public async Task<Result<bool>> Remove(RemoveTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<bool>.Failure(TransactionErrors.NotFound(request.Id));
        
        dbContext.Transactions.Remove(entity);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.Success(true);
    }
}