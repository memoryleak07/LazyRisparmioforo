namespace Risparmioforo.Domain.Transaction;

public class TransactionItem
{
    public string Item { get; set; }
    public decimal Price { get; set; }
}

public class Transaction
{
    public int Id { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionOperation Operation { get; set; }
    public TransactionMerchant? Merchant { get; set; }
    public ICollection<TransactionItem>? Items { get; set; }
}