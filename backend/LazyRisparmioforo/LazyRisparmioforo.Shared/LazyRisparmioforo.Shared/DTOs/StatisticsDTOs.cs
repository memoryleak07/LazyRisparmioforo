namespace LazyRisparmioforo.Shared.DTOs;

public class SummaryDto
{
    public decimal Income { get; init; }
    public decimal Expense { get; init; }
}

public class CategoryAmountDto
{
    public int CategoryId { get; init; }
    public decimal Amount { get; init; }
}