namespace Sparrow.Web.Models
{
    public class PagedListModel
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
    }
}