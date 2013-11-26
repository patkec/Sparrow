using System;
using System.Linq.Expressions;
using AutoMapper;
using NHibernate.Criterion;
using Sparrow.Domain.Models;
using Sparrow.Web.Infrastructure;
using Sparrow.Web.Models.Customers;

namespace Sparrow.Web.Controllers
{
    public class CustomersController : CrudApiController<Customer, CustomerViewModel, CustomerAddModel, CustomerEditModel>
    {
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
