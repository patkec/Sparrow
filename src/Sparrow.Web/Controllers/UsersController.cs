using System;
using System.Linq.Expressions;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models.Users;

namespace Sparrow.Web.Controllers
{
    public class UsersController : CrudApiController<User, UserViewModel, UserAddModel, UserEditModel>
    {
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

        protected override Expression<Func<User, bool>> CreateFilter(string filter)
        {
            return (user => user.Name.IsInsensitiveLike(filter, MatchMode.Anywhere));
        }
    }
}
