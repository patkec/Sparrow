using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Users;
using Sparrow.Web.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Sparrow.Web.Controllers
{
    public class UsersController : CrudApiController<User, UserViewModel, UserDetailsViewModel, UserAddModel, UserEditModel>
    {
        private const string ResourceName = "User";

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
            return (user => user.FirstName.IsInsensitiveLike(filter, MatchMode.Anywhere) || user.LastName.IsInsensitiveLike(filter, MatchMode.Anywhere));
        }

        [ClaimsAuthorize(ResourceActionName.Details, ResourceName)]
        public override IHttpActionResult Get(Guid id)
        {
            return base.Get(id);
        }

        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
        public override PagedListModel<UserViewModel> Get([FromUri] PagedListRequestModel requestModel)
        {
            return base.Get(requestModel);
        }

        [ClaimsAuthorize(ResourceActionName.Delete, ResourceName)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [ClaimsAuthorize(ResourceActionName.Delete, ResourceName)]
        public override IHttpActionResult DeleteMany([FromUri] IEnumerable<Guid> ids)
        {
            return base.DeleteMany(ids);
        }

        [ClaimsAuthorize(ResourceActionName.Create, ResourceName)]
        public override IHttpActionResult Post(UserAddModel model)
        {
            return base.Post(model);
        }

        [ClaimsAuthorize(ResourceActionName.Update, ResourceName)]
        public override IHttpActionResult Put(UserEditModel model)
        {
            return base.Put(model);
        }
    }
}
