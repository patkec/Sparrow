﻿using System;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sparrow.Api.Security;
using Sparrow.Infrastructure.Tasks;
using StructureMap;

namespace Sparrow.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfig.Configure();
            DomainEventConfig.RegisterEvents();

            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;
        }

        protected void Application_EndRequest(object sender, EventArgs args)
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();

            TaskExecutor.StartExecuting();
        }

        private void FederatedAuthentication_FederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
        {
            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager = new AuthorizationManager();
        }
    }
}
