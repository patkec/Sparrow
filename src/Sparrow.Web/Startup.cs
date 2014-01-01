using Owin;
using Sparrow.Web.Security;

namespace Sparrow.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
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