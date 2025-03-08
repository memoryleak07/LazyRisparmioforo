using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Shared.Shared;

namespace CategoryService;

public interface ICategoryService
{
    Task<ICollection<Category>> AllAsync(CancellationToken cancellationToken);
    Task<Result<CategoryPredictResult>> PredictAsync(CategoryPredictCommand command, CancellationToken cancellationToken);
    Task<Result<ICollection<CategoryPredictResult>>> PredictBatchAsync(CategoryPredictBatchCommand command, CancellationToken cancellationToken);
}