using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web.Controllers
{
    public class UsersController : CrudApiController<User, UserViewModel, UserAddModel, UserEditModel>
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

        protected override void OnEntityCreated(User entity)
        {
            var viewModel = Mapper.Map<UserViewModel>(entity);
            AdminHub.Clients.All.userCreated(viewModel);
        }

        protected override void OnEntityUpdated(User entity)
        {
            var viewModel = Mapper.Map<UserViewModel>(entity);
            AdminHub.Clients.All.userUpdated(viewModel);
        }

        protected override void OnEntityDeleted(User entity)
        {
            var viewModel = Mapper.Map<UserViewModel>(entity);
            AdminHub.Clients.All.userDeleted(viewModel);
        }
    }
}
