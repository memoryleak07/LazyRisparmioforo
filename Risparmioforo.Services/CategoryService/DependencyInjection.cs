using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.CategoryService;

public static class DependencyInjection
{
    public static IServiceCollection AddCategoryService(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();

        return services;
    }
}