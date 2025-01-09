namespace Risparmioforo.Domain.Transaction;

public class TransactionMerchant
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public MerchantType MerchantType { get; set; } = MerchantType.Undefined;
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}