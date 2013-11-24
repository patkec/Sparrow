using System;

namespace Sparrow.Web.Models.Customers
{
    public class CustomerEditModel : CustomerAddModel, IEditModel
    {
        /// <summary>
        /// Gets or sets the customer identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}