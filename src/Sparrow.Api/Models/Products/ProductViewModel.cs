using System;

namespace Sparrow.Api.Models.Products
{
    public class ProductViewModel
    {
        /// <summary>
        /// Gets or sets the product identification.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the product title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the product description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        public decimal Price { get; set; }
    }
}