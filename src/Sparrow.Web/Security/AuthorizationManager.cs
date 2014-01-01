using System.Linq;
using System.Security.Claims;

namespace Sparrow.Web.Security
{
    public class AuthorizationManager: ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var action = context.Action.First().Value;

            switch (action)
            {
                case ResourceActionName.Create:
                case ResourceActionName.Update:
                case ResourceActionName.Delete:
                    return context.Principal.IsInRole("admin");
                case ResourceActionName.List:
                case ResourceActionName.Details:
                    return context.Principal.IsInRole("user");
            }

            return false;
        }
    }
}