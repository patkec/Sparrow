using System;

namespace Sparrow.Web.Models.Users
{
    public class UserEditModel : UserEditModelBase, IEditModel
    {
        /// <summary>
        /// Gets or sets the user identification.
        /// </summary>
        public Guid Id { get; set; }
    }
}