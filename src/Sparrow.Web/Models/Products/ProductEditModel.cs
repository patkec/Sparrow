using System;

namespace Sparrow.Web.Models.Products
{
    public class ProductEditModel : ProductAddModel, IEditModel
    {
        /// <summary>
        /// Gets or sets the product identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}