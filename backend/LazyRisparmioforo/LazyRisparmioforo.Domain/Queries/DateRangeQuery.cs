namespace LazyRisparmioforo.Domain.Queries;

public record DateRangeQuery(
    DateOnly FromDate,
    DateOnly ToDate);