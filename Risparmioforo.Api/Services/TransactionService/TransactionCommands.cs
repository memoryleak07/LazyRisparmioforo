namespace Risparmioforo.Api.Services.TransactionService;

public class CreateTransactionCommand
{
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class UpdateTransactionCommand
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class RemoveTransactionCommand
{
    public int Id { get; set; }
}