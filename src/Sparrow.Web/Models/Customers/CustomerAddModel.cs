using System.ComponentModel.DataAnnotations;

namespace Sparrow.Web.Models.Customers
{
    public class CustomerAddModel
    {
        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the customer e-mail.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the customer rating.
        /// </summary>
        [Range(0, 10)]
        public int Rating { get; set; }
    }
}