using Risparmioforo.Shared.Base;

namespace Risparmioforo.Shared.Extensions;

public static class PaginationExtensions
{
    public static Pagination<T> ToPagination<T>(
        this ICollection<T> items, int pageIndex, int pageSize, int totalItemsCount)
        => new(items, pageIndex, pageSize, totalItemsCount);
}