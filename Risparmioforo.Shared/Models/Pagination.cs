namespace Risparmioforo.Shared.Models;

public class Pagination<T>(ICollection<T> items, int pageIndex, int pageSize, int totalItemsCount)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int TotalItemsCount { get; set; } = totalItemsCount;
    public int TotalPagesCount { get; set; } = (int)Math.Ceiling((double)totalItemsCount / pageSize) - 1;
    public ICollection<T>? Items { get; set; } = items;
}