using System.ComponentModel.DataAnnotations;

namespace Sparrow.Web.Models.Users
{
    public class UserAddModel: UserEditModelBase
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        public string UserName { get; set; }
    }
}