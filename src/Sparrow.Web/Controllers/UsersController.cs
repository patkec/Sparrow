using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.SignalR;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Hubs;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web.Controllers
{
    public class UsersController : SessionApiController
    {
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

            var usersQuery = Session.QueryOver<User>()
                .Skip(usersToSkip)
                .Take(requestModel.PageSize);
            if (!string.IsNullOrEmpty(requestModel.Sort))
                usersQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));

            var users = usersQuery.Future();
            var totalItems = usersQuery.ToRowCountQuery().FutureValue<int>();

            return new UserPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Users = Mapper.Map<IEnumerable<UserViewModel>>(users)
            };
        }

        /// <summary>
        /// Gets a user by id.
        /// </summary>
        public HttpResponseMessage Get(Guid id)
        {
            var user = Session.Get<User>(id);

            if (user == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<UserViewModel>(user);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Post(UserAddModel model)
        {
            var user = Mapper.Map<User>(model);
            Session.Save(user);

            GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients.All.sendMessage(string.Format("New user was added: {0}", model.Name));

            var viewModel = Mapper.Map<UserViewModel>(user);
            return Request.CreateResponse(HttpStatusCode.Created, viewModel);
        }

        /// <summary>
        /// Creates or updates a user.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Put(UserEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            var user = Session.Load<User>(model.Id);
            Mapper.Map(model, user);
            Session.Update(user);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes a user with id.
        /// </summary>
        public HttpResponseMessage Delete(Guid id)
        {
            var userToDelete = Session.Get<User>(id);
            if (userToDelete != null)
                Session.Delete(userToDelete);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
