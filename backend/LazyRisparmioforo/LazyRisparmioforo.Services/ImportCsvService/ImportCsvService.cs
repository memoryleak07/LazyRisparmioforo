using System.Globalization;
using CategoryService;
using CsvHelper;
using CsvHelper.Configuration;
using ImportCsvService.Models;
using LazyRisparmioforo.Domain.Commands;
using LazyRisparmioforo.Domain.Constants;
using LazyRisparmioforo.Domain.Entities;
using LazyRisparmioforo.Infrastructure.Data;
using LazyRisparmioforo.Shared.Shared;
using Microsoft.Extensions.Logging;

namespace ImportCsvService;

public class ImportCsvService(
    ApplicationDbContext dbContext,
    ICategoryService categoryService,
    ILogger<ImportCsvService> logger)
    : IImportCsvService
{
    private static readonly CultureInfo CultureInfo = new("it-IT");
    private readonly CsvConfiguration _unicreditCsvConfiguration = new(CultureInfo)
    {
        Delimiter = ";",
        ReadingExceptionOccurred = args => 
        {
            logger.LogError(args.Exception.Message, args.Exception);
            return true;
        }
    };
    
    public async Task<Result> ImportCsvAsync(UploadFileCommand command, CancellationToken cancellationToken)
    {
        using var csv = new CsvReader(new StreamReader(command.FileStream), _unicreditCsvConfiguration);
        
        csv.Context.RegisterClassMap<UnicreditCsvModelMap>();

        try
        {
            var records = csv.GetRecords<UnicreditCsvModel>().ToList();
            var transactions = records.ToTransactions();

            if (transactions.Count == 0)
                return Result.Failure(Errors.CollectionNullOrEmpty);

            transactions = await AssignCategoryToTransactionsAsync(transactions, cancellationToken);

            bool success = await InsertTransactionsAsync(transactions, cancellationToken);

            return success ? Result.Success() : Result.Failure(Errors.InsertError);
        }
        catch (CsvHelperException csvHelperException)
        {
            logger.LogError(csvHelperException, "Exception occured: {Message}", csvHelperException.Message);
            var rowNumber = csvHelperException.Context?.Parser?.Row;
            return Result.Failure(Errors.ParserError(csvHelperException.Message, rowNumber));
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            return Result.Failure(Errors.UnexpectedError);
        }
    }
    
    private async Task<ICollection<Transaction>> AssignCategoryToTransactionsAsync(ICollection<Transaction> transactions, CancellationToken cancellationToken)
    {
        foreach (var transaction in transactions)
        {
            var command = new CategoryPredictCommand(transaction.Description);
            var result = await categoryService.PredictAsync(command, cancellationToken);
            if (!result.IsSuccess)
                continue;
            
            transaction.CategoryId = result.Value.Id;
        }
        
        return transactions;
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
            await dbContextTransaction.RollbackAsync(cancellationToken);
            logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            return false;
        }
    }
}