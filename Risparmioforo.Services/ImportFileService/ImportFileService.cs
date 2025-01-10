using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Services.DocumentIntelligenceService;
using Risparmioforo.Services.UnicreditCsvService;
using Risparmioforo.Shared.Base;
using Transaction = Risparmioforo.Domain.Transaction.Transaction;

namespace Risparmioforo.Services.ImportFileService;

public class ImportFileService(
    ApplicationDbContext dbContext,
    IUnicreditCsvService unicreditCsvService,
    IDocumentIntelligenceService documentIntelligenceService,
    ILogger<ImportFileService> logger,
    IImageValidator imageValidator,
    ICsvValidator csvValidator)
     : IImportFileService
{
    public async Task<Result<bool>> ImportCsvAsync(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await csvValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<bool>.Failure(TransactionErrors.ValidationErrors(errors));
        }

        var readCsvResult = await unicreditCsvService.ReadCsvAsync(request.FileBytes, cancellationToken);
        if (!readCsvResult.IsSuccess)
        {
            return Result<bool>.Failure(readCsvResult.Error!);
        }

        var transactions = readCsvResult.Value;
        if (transactions is null || transactions.Count == 0)
        {
            return Result<bool>.Failure(TransactionErrors.CollectionNullOrEmpty);
        }

        return await InsertTransactionsAsync(transactions, cancellationToken);
    }

    public async Task<Result<bool>> ImportDocumentsAsync(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await imageValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<bool>.Failure(TransactionErrors.ValidationErrors(errors));
        }

        var readDocumentsResult = await documentIntelligenceService.ReadReceiptDocumentsAsync(request.FileBytes, cancellationToken);
        if (!readDocumentsResult.IsSuccess)
        {
            return Result<bool>.Failure(readDocumentsResult.Error!);
        }
        
        var transactions = readDocumentsResult.Value;
        if (transactions is null || transactions.Count == 0)
        {
            return Result<bool>.Failure(TransactionErrors.CollectionNullOrEmpty);
        }
        
        return await InsertTransactionsAsync(transactions, cancellationToken);
    }
    
    private async Task<Result<bool>> InsertTransactionsAsync(ICollection<Transaction> transactions, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            dbContext.AddRange(transactions);
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
            
            logger.LogInformation("Transactions inserted successfully.");
            
            return Result<bool>.Success(true);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            await dbContextTransaction.RollbackAsync(cancellationToken);
            
            return Result<bool>.Failure(new Error("SomeErrorMessageHere", "ErrorCodeHere"));
        }
    }
}