using System.ComponentModel.DataAnnotations;

namespace Sparrow.Web.Models.Users
{
    public class UserAddModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        [Required]
        public string Role { get; set; }
        /// <summary>
        /// Gets or sets user's e-mail address.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}