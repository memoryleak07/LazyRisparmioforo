namespace LazyRisparmioforo.Domain.Commands;

public record StatRequestCommand(
    DateOnly FromDate,
    DateOnly ToDate);