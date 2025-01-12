using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Risparmioforo.Services.ImportFileService;

namespace Risparmioforo.Services.CategoryService;

public static class DependencyInjection
{
    public static void AddCategoryService(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();

        services.AddTransient<ICategoryValidator, CategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<ImportFileValidators>();
    }
}