namespace LazyRisparmioforo.Domain.Commands;

public class PagedSearchCommand
{
    public string? Query { get; init; } = null;
    public int PageIndex { get; init; } = 0;
    public int PageSize { get; init; } = 10;
}
// public record PagedSearchCommand(
//     string? Query = null,
//     int PageIndex = 0,
//     int PageSize = 10);