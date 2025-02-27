using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Shared;
using LazyRisparmioforo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace StatisticsService;

public class StatisticService(
    ApplicationDbContext dbContext,
    ILogger<StatisticService> logger)
    : IStatisticService
{
    /// <summary>
    /// Returns the total amount spent within a specified date range.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<decimal>> TotalAmountAsync(StatCommand command, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.Flow == Flow.Expense &&
                transaction.RegistrationDate >= command.FromDate &&
                transaction.RegistrationDate <= command.ToDate)
            .SumAsync(x => x.Amount, cancellationToken);
        
        return Result.Success(items);
    } 
    
    /// <summary>
    /// Returns the total amount spent per category within a specified date range.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ICollection<StatResult>>> SpentPerCategoryAsync(StatCommand command, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.Flow == command.Flow &&
                transaction.RegistrationDate >= command.FromDate &&
                transaction.RegistrationDate <= command.ToDate)
            .GroupBy(x => x.CategoryId)
            .Select(grouping => new
                {
                    CategoryId = grouping.Key,
                    Amounts = grouping.Select(x => x.Amount)
                }
            )
            .ToListAsync(cancellationToken);

        var results = items
            .Select(x => new StatResult(x.CategoryId, x.Amounts.Sum()))
            .OrderBy(x => x.Amount)
            .ToList();
        
        return results;
    }
}