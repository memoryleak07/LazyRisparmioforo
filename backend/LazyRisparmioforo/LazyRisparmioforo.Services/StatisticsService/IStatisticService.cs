using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Shared;

namespace StatisticsService;

public interface IStatisticService
{
    Task<Result<decimal>> TotalAmountAsync(StatCommand command, CancellationToken cancellationToken);
    Task<Result<ICollection<StatResult>>> SpentPerCategoryAsync(StatCommand command, CancellationToken cancellationToken);
}