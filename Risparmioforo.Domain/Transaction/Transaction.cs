namespace Risparmioforo.Domain.Transaction;

public class Transaction
{
    public int Id { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Card { get; set; } = string.Empty;
    public TransactionType Type { get; set; } = TransactionType.Undefined;
    public TransactionMethod Method { get; set; } = TransactionMethod.Undefined;
    public TransactionOperation Operation { get; set; } = TransactionOperation.Undefined;
    public TransactionMerchant? Merchant { get; set; }
    // public MerchantType MerchantType { get; set; } = MerchantType.Undefined;
    public ICollection<TransactionItem>? Items { get; set; }
}