using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public static class DependencyInjection
{
    private static readonly string Endpoint = "https://documentintelligenceinstance1.cognitiveservices.azure.com/";
    private static readonly string Key = "azureKey";
    
    public static void AddDocumentIntelligenceService(this IServiceCollection services)
    {
        AzureKeyCredential credential = new AzureKeyCredential(Key);
        DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(Endpoint), credential);
        services.AddSingleton(client);
        
        services.AddTransient<IDocumentIntelligenceService, DocumentIntelligenceService>();
    }
}
