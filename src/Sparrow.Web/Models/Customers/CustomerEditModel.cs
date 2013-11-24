using System;

namespace Sparrow.Web.Models.Customers
{
    public class CustomerEditModel : CustomerAddModel
    {
        /// <summary>
        /// Gets or sets the customer identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}