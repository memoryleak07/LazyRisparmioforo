using Risparmioforo.Shared.Base;

namespace Risparmioforo.Domain.Transaction;

public abstract class TransactionErrors
{
    public static Error NotFound(int id) => new(
        $"The transaction with ID = '{id}' was not found.",
        "Transaction.NotFound");
    
    public static Error CollectionNullOrEmpty => new(
        "The collection is null or empty.",
        "Transaction.CollectionNullOrEmpty");
    
    public static Error NotUnique => new(
        "The transaction is not unique.",
        "Transaction.NotUnique");
    
    public static Error InsertError => new(
        "An error occurred while inserting the transactions.",
        "Transaction.InsertError");
    
    public static Error ValidationErrors(IEnumerable<string> validationErrors) => new(
        string.Join(", ", validationErrors),
        "Transaction.ValidationErrors");
    
    public static Error CsvParsingError(string message, int? row = null) => new(
        $"Error parsing CSV at row {(row.HasValue ? row : "unknown")}: {message}",
        "Transaction.CsvParsingError");

    public static Error CsvGenericError(Exception exception) => new(
        exception.Message,
        "Transaction.CsvGenericError");
}
