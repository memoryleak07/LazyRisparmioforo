using LazyRisparmioforo.Domain.Constants;

namespace LazyRisparmioforo.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }
    public Flow Flow { get; set; } = Flow.Undefined;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public DateOnly ValueDate{ get; set; }
    public Category Category { get; set; }
}