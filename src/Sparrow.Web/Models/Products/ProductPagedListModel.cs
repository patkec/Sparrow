using System.Collections.Generic;

namespace Sparrow.Web.Models.Products
{
    public class ProductPagedListModel : PagedListModel
    {
        /// <summary>
        /// Gets or sets a list of products in current page.
        /// </summary>
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}