using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;
using StructureMap;

namespace Sparrow.Api.Infrastructure
{
    public class NHibernateApiFilter : ActionFilterAttribute
    {
        private readonly IContainer _container;

        public NHibernateApiFilter(IContainer container)
        {
            _container = container;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var sessionController = actionContext.ControllerContext.Controller as ISessionController;
            if (sessionController == null)
                return;

            sessionController.Session = _container.GetInstance<ISession>();
            sessionController.Session.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var sessionController = actionExecutedContext.ActionContext.ControllerContext.Controller as ISessionController;
            if (sessionController == null)
                return;

            var session = sessionController.Session;
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