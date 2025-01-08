using System.ComponentModel;
using Risparmioforo.Shared.Models;

namespace Risparmioforo.Shared.Extensions;

/// <summary>
/// EntityFrameworkCore extensions for IQueryable&lt;T&gt;.
/// </summary>
public static class PaginationLinqExtensions
{
    /// <summary>
    /// Converts an IQueryable&lt;T&gt; into a PagedList&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">T should be a class.</typeparam>
    /// <param name="allItems">An IQueryable with all the items present in the list.</param>
    /// <param name="currentPageNumber">The current page number.</param>
    /// <param name="pageSize">The number of elements for a page.</param>
    /// <param name="sortColumn">The name of the property that was used to sort the list.</param>
    /// <param name="sortDirection">The sorting direction associated to the SortBy property.</param>
    public static Pagination<T>  ToPagedList<T>(
        this IQueryable<T> allItems, 
        int currentPageNumber, 
        int pageSize, 
        string sortColumn, 
        ListSortDirection sortDirection
        ) where T : class
    {
        if (currentPageNumber < 1)
            currentPageNumber = 1;

        var totalNumberOfItems = allItems.Count();
        var totalNumberOfPages = totalNumberOfItems == 0 ? 1 : (totalNumberOfItems / pageSize + (totalNumberOfItems % pageSize > 0 ? 1 : 0));
        if (currentPageNumber > totalNumberOfPages)
            currentPageNumber = totalNumberOfPages;

        var itemsToSkip = (currentPageNumber - 1) * pageSize;
        var pagedItems = allItems.Skip(itemsToSkip).Take(pageSize).ToList();

        return new Pagination<T>(pagedItems, currentPageNumber, pageSize, totalNumberOfItems, sortColumn, sortDirection);
    }
}