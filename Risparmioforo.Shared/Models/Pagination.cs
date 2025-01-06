namespace Risparmioforo.Shared.Models;

public class Pagination<T>(ICollection<T> items, int pageIndex, int pageSize)
{
    public int TotalItemsCount { get; set; } = items.Count;
    public int PageSize { get; set; } = pageSize;
    public int PageIndex { get; set; } = pageIndex;
    public int TotalPagesCount { get; set; } = (int)Math.Ceiling((double)items.Count / pageSize);
    public ICollection<T>? Items { get; set; } = items;
}