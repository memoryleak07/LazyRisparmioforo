using Risparmioforo.Services.ImportFileService;
using Risparmioforo.Services.TransactionService;

namespace Risparmioforo.Tests;

public static class DependencyInjection
{
    public static IServiceCollection AddRequiredServices(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}"));

        services.AddTransactionService();
        services.AddImportFileService();

        return services;
    }
}