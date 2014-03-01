using System;

namespace Sparrow.Api.Models.Users
{
    public class UserDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Gets or sets user's e-mail address.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the location of the user.
        /// </summary>
        public string Location { get; set; }
    }
}