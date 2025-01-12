using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Category;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Infrastructure.Data;
using Risparmioforo.Services.CategoryService;
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
    public async Task<Result<Pagination<TransactionDto>>> ImportCsvAsync(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await csvValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Pagination<TransactionDto>>.Failure(TransactionErrors.ValidationErrors(errors));
        }

        var readCsvResult = await unicreditCsvService.ReadCsvAsync(request.FileBytes, cancellationToken);
        if (!readCsvResult.IsSuccess)
        {
            return Result<Pagination<TransactionDto>>.Failure(readCsvResult.Error!);
        }

        var transactions = readCsvResult.Value;
        if (transactions is null || transactions.Count == 0)
        {
            return Result<Pagination<TransactionDto>>.Failure(TransactionErrors.CollectionNullOrEmpty);
        }

        Category[] categories = await dbContext.Categories.AsNoTracking().ToArrayAsync(cancellationToken);
        transactions.SetCategories(categories);
        
        if (!await InsertTransactionsAsync(transactions, cancellationToken))
        {
            Result<Pagination<TransactionDto>>.Failure(TransactionErrors.InsertError);
        }
        
        var dto = transactions.ToTransactionsDto();
        return Result<Pagination<TransactionDto>>.Success(dto.ToPagination(pageIndex: 0, pageSize: 10, transactions.Count));
    }

    public async Task<Result<Pagination<TransactionDto>>> ImportReceiptDocumentsAsync(ImportFileCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await imageValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage);
            return Result<Pagination<TransactionDto>>.Failure(TransactionErrors.ValidationErrors(errors));
        }

        var readDocumentsResult = await documentIntelligenceService.ReadReceiptDocumentsAsync(request.FileBytes, cancellationToken);
        if (!readDocumentsResult.IsSuccess)
        {
            return Result<Pagination<TransactionDto>>.Failure(readDocumentsResult.Error!);
        }
        
        var transactions = readDocumentsResult.Value;
        if (transactions is null || transactions.Count == 0)
        {
            return Result<Pagination<TransactionDto>>.Failure(TransactionErrors.CollectionNullOrEmpty);
        }

        if (!await InsertTransactionsAsync(transactions, cancellationToken))
        {
            return Result<Pagination<TransactionDto>>.Failure(TransactionErrors.InsertError);
        }

        var dto = transactions.ToTransactionsDto();
        return Result<Pagination<TransactionDto>>.Success(dto.ToPagination(pageIndex: 0, pageSize: 10, transactions.Count));
    }
    
    private async Task<bool> InsertTransactionsAsync(ICollection<Transaction> transactions, CancellationToken cancellationToken)
    {
        await using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            dbContext.AddRange(transactions);
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
            
            logger.LogInformation("Transactions inserted successfully.");
            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            await dbContextTransaction.RollbackAsync(cancellationToken);
            return false;
        }
    }
}