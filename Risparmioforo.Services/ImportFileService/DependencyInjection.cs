using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Risparmioforo.Services.DocumentIntelligenceService;
using Risparmioforo.Services.UnicreditCsvService;

namespace Risparmioforo.Services.ImportFileService;

public static class DependencyInjection
{
    public static void AddImportFileService(this IServiceCollection services)
    {
        services.AddTransient<IImportFileService, ImportFileService>();
        
        services.AddTransient<IUnicreditCsvService, UnicreditCsvService.UnicreditCsvService>();
        
        services.AddTransient<IDocumentIntelligenceService, DocumentIntelligenceService.DocumentIntelligenceService>();
        
        services.AddTransient<ICsvValidator, ImportFileValidators.ImportFileCsvCommandValidator>();
        services.AddTransient<IImageValidator, ImportFileValidators.ImportFileImageCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<ImportFileValidators>();
    }
}