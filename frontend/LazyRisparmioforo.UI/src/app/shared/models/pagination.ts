export interface Pagination<T> {
  pageIndex: number;
  pageSize: number;
  totalItemsCount: number;
  totalPagesCount: number;
  items: T[];
}
