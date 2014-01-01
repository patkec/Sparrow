using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Products;
using Sparrow.Web.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Sparrow.Web.Controllers
{
    public class ProductsController : CrudApiController<Product, ProductViewModel, ProductViewModel, ProductAddModel, ProductEditModel>
    {
        private const string ResourceName = "Product";

        protected override void OnEntityCreated(Product entity)
        {
            var viewModel = Mapper.Map<ProductViewModel>(entity);
            AdminHub.Clients.All.productCreated(viewModel);
        }

        protected override void OnEntityUpdated(Product entity)
        {
            var viewModel = Mapper.Map<ProductViewModel>(entity);
            AdminHub.Clients.All.productUpdated(viewModel);
        }

        protected override void OnEntityDeleted(Product entity)
        {
            var viewModel = Mapper.Map<ProductViewModel>(entity);
            AdminHub.Clients.All.productDeleted(viewModel);
        }

        protected override Expression<Func<Product, bool>> CreateFilter(string filter)
        {
            return (product => product.Title.IsInsensitiveLike(filter, MatchMode.Anywhere));
        }

        [ClaimsAuthorize(ResourceActionName.Details, ResourceName)]
        public override IHttpActionResult Get(Guid id)
        {
            return base.Get(id);
        }

        [ClaimsAuthorize(ResourceActionName.List, ResourceName)]
        public override PagedListModel<ProductViewModel> Get([FromUri] PagedListRequestModel requestModel)
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
        public override IHttpActionResult Post(ProductAddModel model)
        {
            return base.Post(model);
        }

        [ClaimsAuthorize(ResourceActionName.Update, ResourceName)]
        public override IHttpActionResult Put(ProductEditModel model)
        {
            return base.Put(model);
        }
    }
}
