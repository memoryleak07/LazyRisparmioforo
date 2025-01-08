using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.TransactionService;

public static class DependencyInjection
{
    public static IServiceCollection AddTransactionService(this IServiceCollection services)
    {
        services.AddTransient<ITransactionService, TransactionService>();
        
        return services;
    }
}