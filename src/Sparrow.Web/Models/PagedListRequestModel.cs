namespace Sparrow.Web.Models
{
    public class PagedListRequestModel
    {
        /// <summary>
        /// Gets or sets a filter string.
        /// </summary>
        public string Filter { get; set; } 
        /// <summary>
        /// Gets or sets page index for current list.
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Gets or sets a number of results to return.
        /// </summary>
        public int PageSize { get; set; }
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