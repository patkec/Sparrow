using System;

namespace Sparrow.Web.Models.Users
{
    public class UserEditModel : UserAddModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}