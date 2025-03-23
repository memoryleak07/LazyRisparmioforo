using LazyRisparmioforo.Domain.Queries;
using LazyRisparmioforo.Shared.DTOs;
using LazyRisparmioforo.Shared.Shared;

namespace StatisticsService;

public interface IStatisticService
{
    Task<Result<SummaryDto>> SummaryAsync(DateRangeQuery command, CancellationToken cancellationToken);
    Task<Result<ICollection<CategoryAmountDto>>> SpentPerCategoryAsync(DateRangeQuery command, CancellationToken cancellationToken);
    Task<Result<ICollection<SummaryMonthlyDto>>> SummaryMonthlyAsync(DateRangeQuery command, CancellationToken cancellationToken);
}