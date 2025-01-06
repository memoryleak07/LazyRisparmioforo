using Risparmioforo.Shared.Base;

namespace Risparmioforo.Domain.Transaction;

public abstract class TransactionErrors
{
    public static Error NotFound(int id) => new(
        "Transaction.NotFound", 
        $"The transaction with ID = '{id}' was not found.");
    
    public static Error NotUnique => new(
        "Transaction.NotUnique", 
        "The transaction is not unique.");
}