using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;

namespace Sparrow.Api.Infrastructure
{
    public class NHibernateApiFilter : ActionFilterAttribute
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateApiFilter(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory");
            _sessionFactory = sessionFactory;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var sessionController = actionContext.ControllerContext.Controller as SessionApiController;
            if (sessionController == null)
                return;

            sessionController.Session = _sessionFactory.OpenSession();
            sessionController.Session.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var sessionController = actionExecutedContext.ActionContext.ControllerContext.Controller as SessionApiController;
            if (sessionController == null)
                return;

            using (var session = sessionController.Session)
            {
                if (session == null)
                    return;
                if (!session.Transaction.IsActive)
                    return;

                if (actionExecutedContext.Exception != null)
                    session.Transaction.Rollback();
                else
                    session.Transaction.Commit();
            }
        }
    }
}