using System.Transactions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Services.UnicreditCsvService;
using Risparmioforo.Shared.Base;
using Transaction = Risparmioforo.Domain.Transaction.Transaction;

namespace Risparmioforo.Services.ImportFileService;

public class ImportFileService(
    ApplicationDbContext dbContext,
    IUnicreditCsvService unicreditCsvService,
    ILogger<ImportFileService> logger,
    IValidator<ImportFileCommand> validator)
     : IImportFileService
{
    public async Task<Result<bool>> ImportAsync(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            
            return Result<bool>.Failure(TransactionErrors.ValidationErrors(errors));
        }

        var transactionsResult = await unicreditCsvService.ReadCsvAsync(request.FileStream, cancellationToken);
        if (!transactionsResult.IsSuccess)
        {
            return Result<bool>.Failure(transactionsResult.Error!);
        }

        var transactions = transactionsResult.Value;
        if (transactions is null || transactions.Count == 0)
        {
            return Result<bool>.Failure(TransactionErrors.CollectionNullOrEmpty);
        }

        await InsertTransactionsAsync(transactions, cancellationToken);
        
        return Result<bool>.Success(true);
    }

    private async Task InsertTransactionsAsync(ICollection<Transaction> transactions, CancellationToken cancellationToken)
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