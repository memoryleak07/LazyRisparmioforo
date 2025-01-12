using Risparmioforo.Domain.Category;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Services.CategoryService;

public interface ICategoryService
{
    Task<Result<Pagination<CategoryDto>>> Search(SearchCommand command, CancellationToken cancellationToken);
    Task<Result<CategoryDto>> Create(CreateCategoryCommand command, CancellationToken cancellationToken);
    Task<Result<CategoryDto>> Update(UpdateCategoryCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveCategoryCommand command, CancellationToken cancellationToken);
}