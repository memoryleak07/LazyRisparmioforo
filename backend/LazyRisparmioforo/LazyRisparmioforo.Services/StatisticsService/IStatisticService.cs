using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Shared.DTOs;
using LazyRisparmioforo.Shared.Shared;

namespace StatisticsService;

public interface IStatisticService
{
    Task<Result<SummaryDto>> SummaryAsync(StatRequestCommand requestCommand, CancellationToken cancellationToken);
    Task<Result<ICollection<CategoryAmountDto>>> SpentPerCategoryAsync(StatRequestCommand requestCommand, CancellationToken cancellationToken);
}