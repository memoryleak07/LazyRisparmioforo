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
    
    public static Error LowConfidenceDocument => new(
        "The operation returned a result but the confidence is too low.",
        "DocumentIntelligence.LowConfidenceDocument");
    
    public static Error TryGetTransactionsError => new(
        "Error converting the documents.",
        "DocumentIntelligence.TryGetTransactionsError");
    
    public static Error ReadReceiptGenericError => new(
        "Exception occurred while reading receipt documents.",
        "DocumentIntelligence.ReceiptGenericError");
}
