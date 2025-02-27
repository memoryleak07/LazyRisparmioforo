using LazyRisparmioforo.Domain.Constants;

namespace LazyRisparmioforo.Domain.Commands;

public record StatCommand(
    DateOnly FromDate,
    DateOnly ToDate,
    Flow Flow);

public record StatResult(
    int CategoryId, 
    decimal Amount);