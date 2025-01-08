using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Risparmioforo.Services.UnicreditCsvService;

namespace Risparmioforo.Services.ImportFileService;

public static class DependencyInjection
{
    public static IServiceCollection AddImportFileService(this IServiceCollection services)
    {
        services.AddTransient<IImportFileService, ImportFileService>();
        
        services.AddTransient<IUnicreditCsvService, UnicreditCsvService.UnicreditCsvService>();

        services.AddValidatorsFromAssemblyContaining<ImportFileCommandValidator>();
        
        return services;
    }
}