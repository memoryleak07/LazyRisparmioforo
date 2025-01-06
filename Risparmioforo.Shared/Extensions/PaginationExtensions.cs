using Risparmioforo.Shared.Models;

namespace Risparmioforo.Shared.Extensions;

public static class PaginationExtensions
{
    public static Pagination<T> ToPagination<T>(this ICollection<T> items, int pageIndex, int pageSize) 
        => new(items, pageIndex, pageSize);
}