using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.AccountService;

public static class DependencyInjection
{
    public static void AddAccountService(this IServiceCollection services)
    {
        services.AddTransient<IAccountService, AccountService>();
        
        services.AddTransient<IAccountValidator, AccountValidator>();
        services.AddValidatorsFromAssemblyContaining<AccountValidator>();
    }
}