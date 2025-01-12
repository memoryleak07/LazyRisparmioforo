using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Category;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;
using Risparmioforo.Shared.Commands;

namespace Risparmioforo.Services.CategoryService;

public class CategoryService(
    ApplicationDbContext dbContext,
    ILogger<CategoryService> logger,
    ICategoryValidator categoryValidator)
     : ICategoryService
{
    public async Task<Result<Pagination<Category>>> Search(SearchCommand command, CancellationToken cancellationToken)
    {
        var query = dbContext.Categories
            .AsNoTracking()
            .Where(string.IsNullOrEmpty(command.Query)
                ? transaction => true
                : transaction => transaction.Name.ToLower() == command.Query.ToLower())
            .AsQueryable();

        var totalItemsCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip(command.PageSize * command.PageIndex)
            .Take(command.PageSize)
            .ToListAsync(cancellationToken);

        var paginatedResults = items.ToPagination(command.PageIndex, command.PageSize, totalItemsCount);

        return Result<Pagination<Category>>.Success(paginatedResults);
    }
    
    public async Task<Result<Category>> Create(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = new Category { Flow = command.Flow, Name = command.Name, Keywords = command.Keywords};
        
        var validationResult = await categoryValidator.ValidateAsync(entity, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Category>.Failure(CategoryErrors.ValidationErrors(errors));
        }
        
        if (await IsNameNotUnique(command.Name))
            return Result<Category>.Failure(CategoryErrors.NotUnique);

        dbContext.Categories.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Category created successfully.");
        
        return Result<Category>.Success(entity);
    }
    
    public async Task<Result<Category>> Update(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (await IsNameNotUnique(command.Name, command.Id))
            return Result<Category>.Failure(CategoryErrors.NotUnique);

        var entity = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<Category>.Failure(CategoryErrors.NotFound(command.Id));

        entity.Name = command.Name;
        entity.Keywords.AddRange(command.Keywords.Except(entity.Keywords));
        
        var validationResult = await categoryValidator.ValidateAsync(entity, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Category>.Failure(CategoryErrors.ValidationErrors(errors));
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Category updated successfully.");
        
        return Result<Category>.Success(entity);
    }
    
    public async Task<Result<bool>> Remove(RemoveCategoryCommand command, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result<bool>.Failure(CategoryErrors.NotFound(command.Id));
        
        dbContext.Categories.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Category removed successfully.");

        return Result<bool>.Success(true);
    }
    
    private async Task<bool> IsNameNotUnique(string name, int? id = null) => 
        await dbContext.Categories.AnyAsync(x => x.Name.ToLower() == name.ToLower() && (id == null || x.Id != id));
}