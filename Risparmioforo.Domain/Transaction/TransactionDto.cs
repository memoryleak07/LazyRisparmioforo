using System.Text.Json;

namespace Risparmioforo.Domain.Transaction;

public class TransactionDto
{
    public int Id { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Flow { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Merchant { get; set; } = string.Empty;
    public string Items { get; set; } = string.Empty;
}

public static class TransactionMappingExtension
{
    public static ICollection<TransactionDto> ToDto(this ICollection<Transaction> transactions) =>
        transactions.Select(t => t.ToDto()).ToList();

    public static TransactionDto ToDto(this Transaction transaction) => 
        new TransactionDto
        {
            Id = transaction.Id,
            RegistrationDate = transaction.RegistrationDate,
            ValueDate = transaction.ValueDate,
            Description = transaction.Description,
            Amount = transaction.Amount,
            CardNumber = transaction.CardNumber,
            Category = transaction.Category,
            Flow = transaction.Flow.ToString(),
            Operation = transaction.Operation.ToString(),
            Method = transaction.Method.ToString(),
            Merchant = transaction.Merchant == null ? "" : JsonSerializer.Serialize(transaction.Merchant),
            Items = transaction.Items?.Count == 0 ? "" : JsonSerializer.Serialize(transaction.Items),
        };
}