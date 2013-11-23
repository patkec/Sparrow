using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sparrow.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Use lowercase for JSON serialization.
            var settings = new JsonSerializerSettings { ContractResolver = new LowercaseContractResolver() };
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
            }
        }
    }
}
