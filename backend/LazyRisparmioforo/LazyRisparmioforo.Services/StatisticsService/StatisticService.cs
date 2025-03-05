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
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<SummaryDto>> SummaryAsync(StatRequestCommand command, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction => 
                transaction.RegistrationDate >= command.FromDate &&
                transaction.RegistrationDate <= command.ToDate)
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
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ICollection<CategoryAmountDto>>> SpentPerCategoryAsync(StatRequestCommand command, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.Flow == Flow.Expense &&
                transaction.RegistrationDate >= command.FromDate &&
                transaction.RegistrationDate <= command.ToDate)
            .GroupBy(x => x.CategoryId)
            .Select(grouping => new CategoryAmountDto
                {
                    CategoryId = grouping.Key,
                    Amount = grouping.Sum(x => x.Amount)
                }
            )
            .OrderBy(x => x.Amount)
            .ToListAsync(cancellationToken);

        return items;
    }
    
    /// <summary>
    /// .
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Result<ICollection<SummaryMonthlyDto>>> SummaryMonthlyAsync(StatRequestCommand command, CancellationToken cancellationToken)
    {
        var items = await dbContext.Transactions
            .AsNoTracking()
            .Where(transaction =>
                transaction.RegistrationDate >= command.FromDate &&
                transaction.RegistrationDate <= command.ToDate)
            .GroupBy(x => x.RegistrationDate.Month)
            .Select(grouping => new SummaryMonthlyDto
                {
                    Month = grouping.Key,
                    Income = grouping.Where(x => x.Flow == Flow.Income).Sum(x => x.Amount),
                    Expense = grouping.Where(x => x.Flow == Flow.Expense).Sum(x => x.Amount)
                }
            )
            .ToListAsync(cancellationToken);
        
        return items;
    }
}