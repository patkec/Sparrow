using System;
using System.Linq.Expressions;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Controllers
{
    public class ProductsController : CrudApiController<Product, ProductViewModel, ProductViewModel, ProductAddModel, ProductEditModel>
    {
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
    }
}
