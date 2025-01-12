using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.TransactionService;

public static class DependencyInjection
{
    public static void AddTransactionService(this IServiceCollection services)
    {
        services.AddTransient<ITransactionService, TransactionService>();
        
        services.AddTransient<ITransactionValidator, TransactionValidator>();
        services.AddValidatorsFromAssemblyContaining<TransactionValidator>();
    }
}