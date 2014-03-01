using System;

namespace Sparrow.Api.Models.Customers
{
    public class CustomerViewModel
    {
        /// <summary>
        /// Gets or sets the customer identification.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the customer e-mail.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the customer rating.
        /// </summary>
        public int Rating { get; set; }
    }
}