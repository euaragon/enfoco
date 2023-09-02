using System;
using enfoco2.Models;

namespace enfoco2.Pagination
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public PaginatedList(List<T> items, int pageIndex, int pageSize, int totalCount, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalCount = totalCount;

            this.AddRange(items);
        }

        public PaginatedList(List<Notice> currentPageNotices, int page, int pageSize, int totalNotices, int totalPages)
        {
            TotalPages = totalPages;
        }

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);
    }

}

