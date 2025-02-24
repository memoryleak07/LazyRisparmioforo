using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Domain.Shared;
using LazyRisparmioforo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Result = LazyRisparmioforo.Domain.Shared.Result;


namespace TransactionService;

public class TransactionService(
    ApplicationDbContext dbContext,
    ILogger<TransactionService> logger)
    : ITransactionService
{
    public async Task<Pagination<Transaction>> SearchAsync(TransactionSearchCommand command, CancellationToken cancellationToken)
    {
        var query = dbContext.Transactions
            .AsNoTracking()
            .Where(string.IsNullOrEmpty(command.Query)
                ? transaction => true
                : transaction => transaction.Description.ToLower().Contains(command.Query.ToLower()))
            .Where(command.Flow == null
                ? transaction => true
                : transaction => transaction.Flow == command.Flow)
            .OrderByDescending(x => x.RegistrationDate)
            .AsQueryable();

        var totalItemsCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(command.PageSize * command.PageIndex)
            .Take(command.PageSize)
            .ToListAsync(cancellationToken);

        // var dto = items.ToDto();
        var paginatedResults = items.ToPagination(command.PageIndex, command.PageSize, totalItemsCount);

        return paginatedResults;
    }

    public async Task<Result<Transaction>> GetAsync(TransactionGetCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(transaction => transaction.Id == command.Id, cancellationToken);

        return entity is null
            ? Result.Failure<Transaction>(Errors.NotFound(command.Id))
            : Result.Success(entity);
    }

    public async Task<Result> CreateAsync(TransactionCreateCommand command, CancellationToken cancellationToken)
    {
        var entity = command.ToEntity();

        dbContext.Transactions.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Transaction created successfully.");

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(TransactionUpdateCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null)
            return Result.Failure(Errors.NotFound(command.Id));

        entity = command.ToEntity(entity);

        dbContext.Entry(entity).State = EntityState.Modified;
        dbContext.Transactions.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Transaction updated successfully.");

        return Result.Success();
    }

    public async Task<Result> RemoveAsync(TransactionRemoveCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Transactions
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (entity is null)
            return Result.Failure(Errors.NotFound(command.Id));

        dbContext.Transactions.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Transaction removed successfully.");

        return Result.Success();
    }
}