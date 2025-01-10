using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public class DocumentIntelligenceService(
    DocumentIntelligenceClient documentIntelligenceClient,
    ILogger<DocumentIntelligenceService> logger
) : IDocumentIntelligenceService
{
    public async Task<Result<ICollection<Transaction>>> ReadReceiptDocumentsAsync(byte[] bytes, CancellationToken cancellationToken)
    {
        try
        {
            Operation<AnalyzeResult> operation = await documentIntelligenceClient.AnalyzeDocumentAsync(
                waitUntil: WaitUntil.Completed,
                modelId: "prebuilt-receipt",
                bytesSource: new BinaryData(bytes),
                cancellationToken: cancellationToken);

            if (!operation.HasValue)
            {
                return Result<ICollection<Transaction>>.Failure(DocumentIntelligenceErrors.OperationFailed);
            }

            if (operation.Value.Documents.Count == 0)
            {
                return Result<ICollection<Transaction>>.Failure(DocumentIntelligenceErrors.NotFoundAnyDocument);
            }

            var transactions = operation.Value.Documents.TryGetTransactions();
            if (transactions is null || transactions.Count == 0)
            {
                return Result<ICollection<Transaction>>.Failure(DocumentIntelligenceErrors.TryGetTransactionsError);
            }

            return Result<ICollection<Transaction>>.Success(transactions);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            return Result<ICollection<Transaction>>.Failure(DocumentIntelligenceErrors.GenericError(exception));
        }
    }
}