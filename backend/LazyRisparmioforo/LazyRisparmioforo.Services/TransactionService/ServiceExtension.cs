using Microsoft.Extensions.DependencyInjection;

namespace TransactionService;

public static class ServiceExtension
{
    public static void AddTransactionService(this IServiceCollection services)
    {
        services.AddTransient<ITransactionService, TransactionService>();
    }
}