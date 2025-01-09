using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public class DocumentIntelligenceService(
    ILogger<DocumentIntelligenceService> logger
) : IDocumentIntelligenceService
{
    public async Task UploadDocumentAsync()
    {
        string endpoint = "";
        string key = "";
        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);

        //sample invoice document
        var testDocument = await File.ReadAllBytesAsync(@"C:\Users\mlucci\Downloads\testScontrino.png");
        var binaryData = new BinaryData(testDocument);
        
        Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(
            WaitUntil.Completed,
            "prebuilt-receipt",
            binaryData);

        if (!operation.HasValue)
        {
            return;
        }

        var transactionResults = operation.Value.Documents.TryGetTransactions();

    }


    private async Task Insert(ICollection<Transaction> collection)
    {
        return;
    }
}