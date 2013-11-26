using System.Collections.Generic;

namespace Sparrow.Web.Models
{
    public class PagedListModel<TModel>
    {
        /// <summary>
        /// Gets or sets total number of items.
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Gets or sets total number of pages.
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Gets or sets page index of returned items.
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Gets or sets size of the returned page.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the items in current page.
        /// </summary>
        public IEnumerable<TModel> Items { get; set; }
    }
}