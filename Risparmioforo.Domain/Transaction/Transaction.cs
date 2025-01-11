using Risparmioforo.Domain.Common;

namespace Risparmioforo.Domain.Transaction;

public class Transaction
{
    public int Id { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Flow Flow { get; set; } = Flow.Undefined;
    public TransactionMethod Method { get; set; } = TransactionMethod.Undefined;
    public TransactionOperation Operation { get; set; } = TransactionOperation.Undefined;
    public TransactionMerchant? Merchant { get; set; }
    public ICollection<TransactionItem>? Items { get; set; }
}