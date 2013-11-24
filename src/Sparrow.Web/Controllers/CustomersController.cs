using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Customers;

namespace Sparrow.Web.Controllers
{
    public class CustomersController : SessionApiController
    {
        /// <summary>
        /// Gets a paged list of customers.
        /// </summary>
        public CustomerPagedListModel Get([FromUri] PagedListRequestModel requestModel)
        {
            requestModel = requestModel ?? new PagedListRequestModel
            {
                PageSize = 20
            };
            var customersToSkip = (requestModel.Page - 1) * requestModel.PageSize;

            var customersQuery = Session.QueryOver<Customer>()
                .Skip(customersToSkip)
                .Take(requestModel.PageSize);
            if (!string.IsNullOrEmpty(requestModel.Sort))
                customersQuery.UnderlyingCriteria.AddOrder(new Order(requestModel.Sort, requestModel.OrderAscending));

            var customers = customersQuery.Future();
            var totalItems = customersQuery.ToRowCountQuery().FutureValue<int>();

            return new CustomerPagedListModel
            {
                Page = requestModel.Page,
                PageSize = requestModel.PageSize,
                TotalItems = totalItems.Value,
                TotalPages = (int)Math.Ceiling(totalItems.Value / (double)requestModel.PageSize),
                Customers = Mapper.Map<IEnumerable<CustomerViewModel>>(customers)
            };
        }

        /// <summary>
        /// Gets a customer by id.
        /// </summary>
        public HttpResponseMessage Get(Guid id)
        {
            var customer = Session.Get<Customer>(id);

            if (customer == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var model = Mapper.Map<CustomerViewModel>(customer);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Post(CustomerAddModel model)
        {
            var customer = Mapper.Map<Customer>(model);
            Session.Save(customer);

            var viewModel = Mapper.Map<CustomerViewModel>(customer);
            return Request.CreateResponse(HttpStatusCode.Created, viewModel);
        }

        /// <summary>
        /// Creates or updates a customer.
        /// </summary>
        [ValidateModel]
        public HttpResponseMessage Put(CustomerEditModel model)
        {
            if (model.Id == Guid.Empty)
                return Post(model);

            var customer = Session.Load<Customer>(model.Id);
            Mapper.Map(model, customer);
            Session.Update(customer);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes a customer with id.
        /// </summary>
        public HttpResponseMessage Delete(Guid id)
        {
            var customerToDelete = Session.Get<Customer>(id);
            if (customerToDelete != null)
                Session.Delete(customerToDelete);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
