using System;

namespace Sparrow.Api.Models
{
    public class PagedListRequestModel
    {
        private int _page = 1;
        private int _pageSize = 20;

        /// <summary>
        /// Gets or sets a filter string.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets page index for current list.
        /// </summary>
        /// <remarks>Page index is 1-based.</remarks>
        public int Page
        {
            get { return _page; }
            set
            {
                _page = Math.Max(value, 1);
            }
        }

        /// <summary>
        /// Gets or sets a number of results to return.
        /// </summary>
        /// <remarks>
        /// Page size can be in range from 1 to 100 items.
        /// </remarks>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = Math.Max(Math.Min(value, 100), 1);
            }
        }

        /// <summary>
        /// Gets or sets a column name by which to sort.
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// Gets or sets the sort order, descending (default) or ascending.
        /// </summary>
        public bool OrderAscending { get; set; }
    }
}