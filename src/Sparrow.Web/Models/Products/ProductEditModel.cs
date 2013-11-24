using System;

namespace Sparrow.Web.Models.Products
{
    public class ProductEditModel : ProductAddModel
    {
        /// <summary>
        /// Gets or sets the product identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}