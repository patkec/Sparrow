using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Customers;
using Sparrow.Web.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Sparrow.Web.Controllers
{
    [Authorize]
    public class CustomersController : CrudApiController<Customer, CustomerViewModel, CustomerViewModel, CustomerAddModel, CustomerEditModel>
    {
        private const string ResourceName = "Customer";

        protected override void OnEntityCreated(Customer entity)
        {
            var viewModel = Mapper.Map<CustomerViewModel>(entity);
            AdminHub.Clients.All.customerCreated(viewModel);
        }

        protected override void OnEntityUpdated(Customer entity)
        {
            var viewModel = Mapper.Map<CustomerViewModel>(entity);
            AdminHub.Clients.All.customerUpdated(viewModel);
        }

        protected override void OnEntityDeleted(Customer entity)
        {
            var viewModel = Mapper.Map<CustomerViewModel>(entity);
            AdminHub.Clients.All.customerDeleted(viewModel);
        }

        protected override Expression<Func<Customer, bool>> CreateFilter(string filter)
        {
            return (customer => customer.Name.IsInsensitiveLike(filter, MatchMode.Anywhere));
        }

        [ClaimsAuthorize(ResourceActionName.Details, ResourceName)]
        public override IHttpActionResult Get(Guid id)
        {
            return base.Get(id);
        }

        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
        public override PagedListModel<CustomerViewModel> Get([FromUri] PagedListRequestModel requestModel)
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
        public override IHttpActionResult Post(CustomerAddModel model)
        {
            return base.Post(model);
        }

        [ClaimsAuthorize(ResourceActionName.Update, ResourceName)]
        public override IHttpActionResult Put(CustomerEditModel model)
        {
            return base.Put(model);
        }
    }
}
