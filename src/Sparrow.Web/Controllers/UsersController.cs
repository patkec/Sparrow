using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web.Controllers
{
    public class UsersController : ApiController
    {
        private readonly List<UserListItemModel> _users = new List<UserListItemModel>();

        public UsersController()
        {
            for (int i = 0; i < 100; i++)
                _users.Add(new UserListItemModel
                {
                    Name = "User " + i,
                    Email = "a@b.com",
                    Role = "test role",
                    Id = Guid.NewGuid()
                });
        }

        public UserPagedListModel Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var usersToSkip = (requestModel.Page - 1)*requestModel.PageSize;

            return new UserPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = _users.Count,
                TotalPages = (int)Math.Ceiling(_users.Count / (double)requestModel.PageSize),
                Users = _users.Skip(usersToSkip).Take(requestModel.PageSize)
            };
        }
    }
}
