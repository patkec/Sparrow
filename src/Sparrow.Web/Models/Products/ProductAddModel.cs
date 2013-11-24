using System.ComponentModel.DataAnnotations;

namespace Sparrow.Web.Models.Products
{
    public class ProductAddModel
    {
        /// <summary>
        /// Gets or sets the product title.
        /// </summary>
        [Required]
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