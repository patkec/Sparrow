using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Users;
using Sparrow.Web.Utils;

namespace Sparrow.Web.Controllers
{
    public class UsersController : ApiController
    {
        private static readonly List<UserViewModel> _users = new List<UserViewModel>();

        static UsersController()
        {
            for (int i = 0; i < 100; i++)
                _users.Add(new UserViewModel
                {
                    Name = "User " + i,
                    Email = "a@b.com",
                    Role = "test role",
                    Id = Guid.NewGuid()
                });
        }

        /// <summary>
        /// Gets a paged list of users.
        /// </summary>
        public UserPagedListModel Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var usersToSkip = (requestModel.Page - 1)*requestModel.PageSize;

            IEnumerable<UserViewModel> users = _users;
            if (!string.IsNullOrEmpty(requestModel.Sort))
                users = requestModel.OrderAscending 
                    ? users.OrderBy(requestModel.Sort) 
                    : users.OrderByDescending(requestModel.Sort);

            return new UserPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = _users.Count,
                TotalPages = (int)Math.Ceiling(_users.Count / (double)requestModel.PageSize),
                Users = users.Skip(usersToSkip).Take(requestModel.PageSize)
            };
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        public HttpResponseMessage Get(Guid id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Post(UserAddModel model)
        {
            var user = new UserViewModel
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                Name = model.Name,
                Role = model.Role
            };
            _users.Add(user);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }

        /// <summary>
        /// Creates or updates a user.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Put(UserEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            _users.RemoveAll(x => x.Id == model.Id);
            _users.Add(new UserViewModel
            {
                Id = model.Id,
                Email = model.Email,
                Name = model.Name,
                Role = model.Role
            });
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes a user with id.
        /// </summary>
        public HttpResponseMessage Delete(Guid id)
        {
            _users.RemoveAll(x => x.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
