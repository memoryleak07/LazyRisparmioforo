using LazyRisparmioforo.Domain.Constants;

namespace LazyRisparmioforo.Domain.Queries;

public record TransactionSearchQuery(
    Flow? Flow,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? Query = null,
    int PageIndex = 0,
    int PageSize = 10 
) : PagedSearchQuery(Query, PageIndex, PageSize);
