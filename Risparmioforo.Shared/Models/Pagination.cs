using System.ComponentModel;

namespace Risparmioforo.Shared.Models;

public class Pagination<T>
{
    public Pagination(
        ICollection<T> items,
        int pageIndex,
        int pageSize, 
        int totalItemsCount)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItemsCount = totalItemsCount;
        TotalPagesCount = (int)Math.Ceiling((double)totalItemsCount / pageSize)
                               - (totalItemsCount > 0 ? 1 : 0);
        Items = items;
    }

    public Pagination(
        ICollection<T> items, 
        int pageIndex, 
        int pageSize, 
        int totalItemsCount, 
        string sortColumn, 
        ListSortDirection sortDirection)
    {
        
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItemsCount { get; set; }
    public int TotalPagesCount { get; set; }
    public ICollection<T>? Items { get; set; }
}