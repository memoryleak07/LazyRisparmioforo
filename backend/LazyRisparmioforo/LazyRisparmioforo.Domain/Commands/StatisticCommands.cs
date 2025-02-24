namespace LazyRisparmioforo.Domain.Commands;

public record StatCommand(
    DateOnly FromDate,
    DateOnly ToDate);

public record StatResult(
    int CategoryId, 
    decimal Amount);