using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public class DocumentIntelligenceService(
    DocumentIntelligenceClient documentAnalysisClient,
    ApplicationDbContext dbContext,
    ILogger<DocumentIntelligenceService> logger
) : IDocumentIntelligenceService
{
    public async Task<Result<bool>> UploadDocumentAsync(StreamReader streamReader, CancellationToken cancellationToken)
    {
        var testDocument = await File.ReadAllBytesAsync(@"C:\Users\Marco\Downloads\scontrino.jpg");

        var binaryData = new BinaryData(testDocument);
        Operation<AnalyzeResult> operation = await documentAnalysisClient.AnalyzeDocumentAsync(
            WaitUntil.Completed,
            "prebuilt-receipt",
            binaryData,
            cancellationToken);

        if (!operation.HasValue)
        {
            return Result<bool>.Failure(new Error("SomeErrorMessage", "UploadDocument.Error"));
        }

        var transactions = operation.Value.Documents.TryGetTransactions();
        if (transactions.Count == 0)
        {
            return Result<bool>.Failure(new Error("SomeErrorMessage", "UploadDocument.Error"));
        }

        await Insert(transactions, cancellationToken);
        
        return Result<bool>.Success(true);
    }


    private async Task Insert(ICollection<Transaction> transactions, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await dbContext.AddRangeAsync(transactions, cancellationToken);
            
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
            
            logger.LogInformation("Transactions inserted successfully.");
        }
        catch (Exception exception)
        {
            logger.LogError(exception.Message, exception);
            await dbContextTransaction.RollbackAsync(cancellationToken);
        }
    }
}