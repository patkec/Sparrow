using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Sparrow.Api.Security;

namespace Sparrow.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });
        }

        private static void ConfigureAuth(IAppBuilder app)
        {
            // validate JWT tokens from AuthorizationServer
            app.UseJsonWebToken(
                issuer: "AS",
                audience: "sparrow",
                signingKey: "OYGY0nAkQUZx1YrT6ler8CI6qbJHyn32tWbCDNuLL+M=");

            // claims transformation
            app.UseClaimsTransformation(new ClaimsTransformer());
        }
    }
}