namespace Risparmioforo.Domain.Transaction;

public class Transaction
{
    public int Id { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionCategory Category { get; set; }
    public TransactionType Type { get; set; }
    public TransactionMerchant? Merchant { get; set; }
}