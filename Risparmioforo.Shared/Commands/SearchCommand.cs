namespace Risparmioforo.Shared.Commands;

public class SearchCommand
{
    public string? Query { get; set; }
    public int PageIndex { get; set; } 
    public int PageSize { get; set; }
}