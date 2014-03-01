using System;

namespace Sparrow.Api.Models.Users
{
    public class UserViewModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string FullName { get; set; }
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