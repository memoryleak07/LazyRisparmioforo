using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public class DocumentIntelligenceService(
    DocumentIntelligenceClient documentIntelligenceClient,
    ApplicationDbContext dbContext,
    ILogger<DocumentIntelligenceService> logger
) : IDocumentIntelligenceService
{
    public async Task<Result<ICollection<Transaction>>> ReadReceiptDocumentsAsync(byte[] bytes, CancellationToken cancellationToken)
    {
        var binaryData = new BinaryData(bytes);
        Operation<AnalyzeResult> operation = await documentIntelligenceClient.AnalyzeDocumentAsync(
            WaitUntil.Completed,
            "prebuilt-receipt",
            binaryData,
            cancellationToken);

        if (!operation.HasValue)
        {
            return Result<ICollection<Transaction>>.Failure(new Error("SomeErrorMessage", "UploadDocument.Error"));
        }

        var transactions = operation.Value.Documents.TryGetTransactions();
        if (transactions is null || transactions.Count == 0)
        {
            return Result<ICollection<Transaction>>.Failure(new Error("SomeErrorMessage", "UploadDocument.Error"));
        }
        
        return Result<ICollection<Transaction>>.Success(transactions);
    }
}