using System;
using System.Linq.Expressions;
using System.Web.Http;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Api.Infrastructure;
using Sparrow.Api.Models.Customers;

namespace Sparrow.Api.Controllers
{
    [Authorize]
    public class CustomersController :
        CrudApiController<Customer, CustomerViewModel, CustomerViewModel, CustomerAddModel, CustomerEditModel>
    {
        protected override string ResourceName { get { return "Customer"; } }

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
    }
}
