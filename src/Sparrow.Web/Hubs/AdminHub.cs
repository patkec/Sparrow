using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Sparrow.Web.Hubs
{
    [HubName("adminHub")]
    public class AdminHub: Hub
    {
        public override Task OnConnected()
        {
            if (Clients.Caller.GetType() == typeof (object))
                return base.OnConnected();
            return base.OnConnected();
        }
    }
}