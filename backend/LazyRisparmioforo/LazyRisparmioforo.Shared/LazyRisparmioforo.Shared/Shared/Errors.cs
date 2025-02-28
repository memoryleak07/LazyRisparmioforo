using LazyRisparmioforo.Domain.Shared;

namespace LazyRisparmioforo.Shared.Shared;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");

    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}

public abstract class Errors
{
    public static Error NotFound(int id) => new(
        "Error.NotFound",
        $"The item with ID = '{id}' was not found.");
    
    public static Error CollectionNullOrEmpty => new(
        "Error.CollectionNullOrEmpty",
        "The collection is null or empty.");
    
    public static Error NotUnique => new(
        "Error.NotUnique",
        "The item is not unique.");
    
    public static Error InsertError => new(
        "Error.InsertError",
        "An error occurred while inserting the item.");    
    
    public static Error CouldNotDeserialize => new(
        "Error.CouldNotDeserialize",
        "An error occurred while deserializing.");
    
    public static Error ParserError(string message, int? row = null)  => new(
        "Error.ParserError",
        $"Error parsing at row {(row.HasValue ? row : "unknown")}: {message}");
    
    public static Error ClientError => new(
        "Error.ClientError",
        "Error occurred while connecting to the client.");
    
    public static Error UnexpectedError => new(
        "Error.UnexpectedError",
        "An unexpected error occurred.");
}
