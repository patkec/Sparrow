using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sparrow.Web.Infrastructure;
using StructureMap;

namespace Sparrow.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(ObjectFactory.GetInstance<NHibernateApiFilter>());

            // Use lowercase for JSON serialization.
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
