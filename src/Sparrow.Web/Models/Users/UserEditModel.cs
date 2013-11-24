using System;

namespace Sparrow.Web.Models.Users
{
    public class UserEditModel : UserAddModel, IEditModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}