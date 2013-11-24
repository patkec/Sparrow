using System.Collections.Generic;

namespace Sparrow.Web.Models.Users
{
    public class UserPagedListModel : PagedListModel
    {
        /// <summary>
        /// Gets or sets a list of users in current page.
        /// </summary>
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}