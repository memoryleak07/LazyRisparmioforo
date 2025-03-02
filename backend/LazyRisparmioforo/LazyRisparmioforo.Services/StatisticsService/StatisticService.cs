using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Infrastructure.Data;
using LazyRisparmioforo.Shared.DTOs;
using LazyRisparmioforo.Shared.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace StatisticsService;

public class StatisticService(
    ApplicationDbContext dbContext,
    ILogger<StatisticService> logger)
    : IStatisticService
{
    /// <summary>
    /// .
    /// </summary>
    /// <param name="requestCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<SummaryDto>> SummaryAsync(StatRequestCommand requestCommand, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => 
                transaction.RegistrationDate >= requestCommand.FromDate &&
                transaction.RegistrationDate <= requestCommand.ToDate)
            .ToListAsync(cancellationToken);

        var dto = new SummaryDto
        {
            Expense = items.Where(t => t.Flow == Flow.Expense).Sum(transaction => transaction.Amount),
            Income = items.Where(t => t.Flow == Flow.Income).Sum(transaction => transaction.Amount),
        };
        
        return dto;
    }

    /// <summary>
    /// Returns the total amount spent per category within a specified date range.
    /// </summary>
    /// <param name="requestCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ICollection<CategoryAmountDto>>> SpentPerCategoryAsync(StatRequestCommand requestCommand, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.RegistrationDate >= requestCommand.FromDate &&
                transaction.RegistrationDate <= requestCommand.ToDate)
            .GroupBy(x => x.CategoryId)
            .Select(grouping => new
                {
                    CategoryId = grouping.Key,
                    Amounts = grouping.Select(x => x.Amount)
                }
            )
            .ToListAsync(cancellationToken);

        var results = items
            .Select(x => new CategoryAmountDto
            {
                CategoryId= x.CategoryId, 
                Amount = x.Amounts.Sum()
            })
            .OrderBy(x => x.Amount)
            .ToList();
        
        return results;
    }
}