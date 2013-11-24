using System.Web.Http;
using NHibernate;

namespace Sparrow.Web.Infrastructure
{
    public class SessionApiController: ApiController
    {
        /// <summary>
        /// Gets the NHibernate session for current request.
        /// </summary>
        protected internal ISession Session { get; internal set; }
    }
}