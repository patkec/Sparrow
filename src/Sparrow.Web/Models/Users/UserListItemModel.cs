using System;

namespace Sparrow.Web.Models.Users
{
    public class UserListItemModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Gets or sets user's e-mail address.
        /// </summary>
        public string Email { get; set; }
    }
}