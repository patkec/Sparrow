using System.Linq;
using System.Web.Http;
using NHibernate.Linq;
using Sparrow.Api.Infrastructure;
using Sparrow.Domain.Models;

namespace Sparrow.Api.Controllers.OData
{
    public class ODataProductsController: SessionODataController
    {
        [Queryable]
        public IQueryable<Product> Get()
        {
            return Session.Query<Product>();
        }
    }
}