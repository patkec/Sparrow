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
using Sparrow.Web.Models.Products;

namespace Sparrow.Web.Controllers
{
    public class ProductsController : SessionApiController
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

        /// <summary>
        /// Gets a product by id.
        /// </summary>
        public HttpResponseMessage Get(Guid id)
        {
            var product = Session.Get<Product>(id);

            if (product == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<ProductViewModel>(product);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Post(ProductAddModel model)
        {
            var product = Mapper.Map<Product>(model);
            Session.Save(product);

            GlobalHost.ConnectionManager.GetHubContext<AdminHub>().Clients.All.sendMessage(string.Format("New product was added: {0}", model.Title));

            var viewModel = Mapper.Map<ProductViewModel>(product);
            return Request.CreateResponse(HttpStatusCode.Created, viewModel);
        }

        /// <summary>
        /// Creates or updates a product.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Put(ProductEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            var product = Session.Load<Product>(model.Id);
            Mapper.Map(model, product);
            Session.Update(product);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes a product with id.
        /// </summary>
        public HttpResponseMessage Delete(Guid id)
        {
            var productToDelete = Session.Get<Product>(id);
            if (productToDelete != null)
                Session.Delete(productToDelete);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
