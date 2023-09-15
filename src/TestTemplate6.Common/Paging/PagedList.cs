using System;
using System.Collections.Generic;

namespace TestTemplate6.Common.Paging
{
    /// <summary>
    /// Paging information.
    /// </summary>
    /// <typeparam name="T">Type of paged elements.</typeparam>
    public class PagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="items">Items on the current page.</param>
        /// <param name="currentPage">Number of the current page.</param>
        /// <param name="totalItems">Total number of items in the result set.</param>
        /// <param name="pageSize">Number of elements on the current page.</param>
        public PagedList(List<T> items, int currentPage, int totalItems, int pageSize)
        {
            CurrentPage = currentPage;
            TotalItems = totalItems;
            TotalPages = Math.Max(1, Convert.ToInt32(Math.Ceiling((decimal)totalItems / pageSize)));
            PageSize = pageSize;
            HasNextPage = CurrentPage < TotalPages;
            HasPreviousPage = CurrentPage > 1;
            Items = items;
            Paging = new Paging(
                CurrentPage,
                TotalPages,
                TotalItems,
                PageSize);
        }

        public Paging Paging { get; }
        public int CurrentPage { get; }
        public int TotalItems { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }

        public List<T> Items { get; }
    }
}
