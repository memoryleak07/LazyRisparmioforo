using Risparmioforo.Domain.Category;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Services.CategoryService;

public interface ICategoryService
{
    Task<Result<Pagination<Category>>> Search(SearchCommand command, CancellationToken cancellationToken);
    Task<Result<Category>> Create(CreateCategoryCommand command, CancellationToken cancellationToken);
    Task<Result<Category>> Update(UpdateCategoryCommand command, CancellationToken cancellationToken);
    Task<Result<bool>> Remove(RemoveCategoryCommand command, CancellationToken cancellationToken);
}