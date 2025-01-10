namespace Risparmioforo.Domain.Transaction;

public class TransactionMerchant
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}