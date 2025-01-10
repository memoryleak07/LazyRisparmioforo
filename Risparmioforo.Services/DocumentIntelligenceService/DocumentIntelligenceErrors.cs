using Risparmioforo.Shared.Base;

namespace Risparmioforo.Services.DocumentIntelligenceService;

public abstract class DocumentIntelligenceErrors
{
    public static Error OperationFailed => new(
        "The operation failed.",
        "DocumentIntelligence.OperationFailed");
    
    public static Error NotFoundAnyDocument => new(
        "The client did not found any document.",
        "DocumentIntelligence.NotFoundAnyDocument");
    
    public static Error TryGetTransactionsError => new(
        "Error converting the document.",
        "DocumentIntelligence.TryGetTransactionsError");
    
    public static Error GenericError(Exception exception) => new(
        exception.Message,
        "DocumentIntelligence.CsvGenericError");
}
