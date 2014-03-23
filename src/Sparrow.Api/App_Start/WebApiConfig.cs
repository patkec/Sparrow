using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Routing.Conventions;
using Microsoft.Data.Edm;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sparrow.Api.Infrastructure;
using Sparrow.Domain.Models;
using StructureMap;

namespace Sparrow.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // We need to expose the DataServiceVersion for data.js to be able to read OData $metadata header. See http://datajs.codeplex.com/workitem/756
            config.EnableCors(new EnableCorsAttribute("*", "*", "*", "DataServiceVersion"));
            config.Filters.Add(ObjectFactory.GetInstance<NHibernateApiFilter>());
            config.Services.Replace(typeof(IHttpControllerSelector), new ControllerSelector(config));

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

            config.Routes.MapODataRoute("DefaultOData", "odata", GetModel());
        }

        private static IEdmModel GetModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Product>("Products");
            builder.Namespace = typeof (Product).Namespace;

            return builder.GetEdmModel();
        }
    }
}
