using System.ComponentModel.DataAnnotations;

namespace Sparrow.Api.Models.Users
{
    public abstract class UserEditModelBase
    {
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
        /// <summary>
        /// Gets or sets user first name.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets user last name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets user location.
        /// </summary>
        public string Location { get; set; }
    }
}