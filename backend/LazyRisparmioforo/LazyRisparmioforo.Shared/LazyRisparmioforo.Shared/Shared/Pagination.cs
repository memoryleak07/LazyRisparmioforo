namespace LazyRisparmioforo.Shared.Shared;

public class Pagination<T>
{
    public Pagination(ICollection<T> items, int pageIndex, int pageSize, int totalItemsCount)
    {
        PageSize = Math.Max(1, pageSize);
        PageIndex = pageIndex;
        TotalItemsCount = totalItemsCount;
        TotalPagesCount = totalItemsCount == 0 ? 0 : (int)Math.Ceiling((double)totalItemsCount / PageSize);
        Items = items;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPagesCount { get; set; }
    public ICollection<T>? Items { get; set; }
}

public static class PaginationExtensions
{
    public static Pagination<T> ToPagination<T>(
        this ICollection<T> items, int pageIndex, int pageSize, int totalItemsCount)
        => new(items, pageIndex, pageSize, totalItemsCount);
}