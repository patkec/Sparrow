using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sparrow.Web.Infrastructure
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            // If there is only a single argument (a model), it is required.
            if (actionContext.ActionArguments.Count == 1)
                if (actionContext.ActionArguments[actionContext.ActionArguments.Keys.First()] == null)
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            base.OnActionExecuting(actionContext);
        }
    }
}