namespace Risparmioforo.Services.TransactionService;

public class SearchTransactionCommand
{
    public string? Query { get; set; }
    public int PageIndex { get; set; } 
    public int PageSize { get; set; }
}

public class CreateTransactionCommand
{
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class UpdateTransactionCommand
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class RemoveTransactionCommand
{
    public int Id { get; set; }
}