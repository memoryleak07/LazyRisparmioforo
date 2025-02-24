using Microsoft.Extensions.DependencyInjection;

namespace CategoryService;

public static class ServiceExtension
{
    public static void AddCategoryService(this IServiceCollection services)
    {
        services.AddTransient<ICategoryService, CategoryService>();
    }
}