using Microsoft.Extensions.DependencyInjection;

namespace ImportCsvService;

public static class ServiceExtension
{
    public static void AddImportCsvService(this IServiceCollection services)
    {
        services.AddTransient<IImportCsvService, ImportCsvService>();
    }
}