using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models;
using Sparrow.Web.Models.Customers;

namespace Sparrow.Web.Controllers
{
    public class CustomersController : CrudApiController<Customer, CustomerViewModel, CustomerAddModel, CustomerEditModel>
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
    }
}
