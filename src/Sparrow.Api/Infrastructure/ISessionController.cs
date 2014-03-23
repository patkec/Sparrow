using NHibernate;

namespace Sparrow.Api.Infrastructure
{
    internal interface ISessionController
    {
        ISession Session { get; set; }
    }
}