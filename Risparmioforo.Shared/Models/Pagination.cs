namespace Risparmioforo.Shared.Models;

public class Pagination<T>
{
    public Pagination(ICollection<T> items, int pageIndex, int pageSize, int totalItemsCount)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItemsCount = totalItemsCount;
        TotalPagesCount = totalItemsCount == 0 ? 0 : (int)Math.Ceiling((double)totalItemsCount / pageSize);
        Items = items;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPagesCount { get; set; }
    public ICollection<T>? Items { get; set; }
}