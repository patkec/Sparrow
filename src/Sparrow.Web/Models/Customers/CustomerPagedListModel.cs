using System.Collections.Generic;
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Models.Customers
{
    public class CustomerPagedListModel : PagedListModel
    {
        /// <summary>
        /// Gets or sets a list of customers in current page.
        /// </summary>
        public IEnumerable<CustomerViewModel> Customers { get; set; }
    }
}