using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Risparmioforo.Domain.Transaction;
using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.UnicreditCsvService;

public class UnicreditCsvService(
    ILogger<UnicreditCsvService> logger)
    : IUnicreditCsvService
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
    
    public async Task<Result<ICollection<Transaction>>> ReadCsvAsync(
        StreamReader streamReader, 
        CancellationToken cancellationToken)
    {
        using var csv = new CsvReader(streamReader, _unicreditCsvConfiguration);
        
        csv.Context.RegisterClassMap<UnicreditCsvModelMap>();

        try
        {
            var records = csv.GetRecords<UnicreditCsvModel>().ToList();
            var transactions = records.ToTransactions();
            
            return Result<ICollection<Transaction>>.Success(transactions);
        }
        catch (CsvHelperException csvHelperException)
        {
            var rowNumber = csvHelperException.Context?.Parser?.Row;
            return Result<ICollection<Transaction>>.Failure(TransactionErrors.CsvParsingError(csvHelperException.Message, rowNumber));
        }
        catch (Exception exception)
        {
            return Result<ICollection<Transaction>>.Failure(TransactionErrors.CsvGenericError(exception));
        }
    }
}