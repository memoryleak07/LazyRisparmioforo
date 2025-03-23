namespace LazyRisparmioforo.Domain.Queries;

public record PagedSearchQuery(
    string? Query,
    int PageIndex,
    int PageSize);