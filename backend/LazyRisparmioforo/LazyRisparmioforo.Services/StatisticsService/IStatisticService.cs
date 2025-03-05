using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Shared.DTOs;
using LazyRisparmioforo.Shared.Shared;

namespace StatisticsService;

public interface IStatisticService
{
    Task<Result<SummaryDto>> SummaryAsync(StatRequestCommand command, CancellationToken cancellationToken);
    Task<Result<ICollection<CategoryAmountDto>>> SpentPerCategoryAsync(StatRequestCommand command, CancellationToken cancellationToken);
    Task<Result<ICollection<SummaryMonthlyDto>>> SummaryMonthlyAsync(StatRequestCommand command, CancellationToken cancellationToken);
}