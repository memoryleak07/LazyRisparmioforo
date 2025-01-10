namespace Risparmioforo.Domain.Transaction;

public class TransactionItem
{
    public int Id { get; set; }
    public string Item { get; set; } = String.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}