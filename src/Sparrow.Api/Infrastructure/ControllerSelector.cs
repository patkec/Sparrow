using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Sparrow.Api.Infrastructure
{
    public class ControllerSelector : DefaultHttpControllerSelector
    {
        public ControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public override string GetControllerName(HttpRequestMessage request)
        {
            var prefix = string.Empty;
            var localPath = request.RequestUri.LocalPath.ToLowerInvariant();
            // All OData controllers must start with OData to differentiate from Api controllers.
            if (localPath.Contains("/odata/") && !localPath.Contains("/odata/$"))
            {
                prefix = "OData";
            }
            return prefix + base.GetControllerName(request);
        }
    }
}