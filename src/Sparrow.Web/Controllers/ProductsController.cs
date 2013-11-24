using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Controllers
{
    public class ProductsController : CrudApiController<Product, ProductViewModel, ProductAddModel, ProductEditModel>
    {
        /// <summary>
        /// Gets a paged list of products.
        /// </summary>
        public ProductPagedListModel Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var productsToSkip = (requestModel.Page - 1) * requestModel.PageSize;

            var productsQuery = Session.QueryOver<Product>()
                .Skip(productsToSkip)
                .Take(requestModel.PageSize);
            if (!string.IsNullOrEmpty(requestModel.Sort))
                productsQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));

            var products = productsQuery.Future();
            var totalItems = productsQuery.ToRowCountQuery().FutureValue<int>();

            return new ProductPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Products = Mapper.Map<IEnumerable<ProductViewModel>>(products)
            };
        }

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
    }
}
