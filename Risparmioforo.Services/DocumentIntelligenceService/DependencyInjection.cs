using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public static class DependencyInjection
{
    private static readonly string Endpoint = "https://documentintelligenceinstance1.cognitiveservices.azure.com/";
    private static readonly string Key = "5zsfi2CmeEF1oR9VXFfkbfNbB6TR5AabyX4CmY6rAuTsFbv9XA1BJQQJ99BAACgEuAYXJ3w3AAALACOGzoc8";
    
    public static void AddDocumentIntelligenceService(this IServiceCollection services)
    {
        AzureKeyCredential credential = new AzureKeyCredential(Key);
        DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(Endpoint), credential);
        services.AddSingleton(client);
        
        services.AddTransient<IDocumentIntelligenceService, DocumentIntelligenceService>();
    }
}