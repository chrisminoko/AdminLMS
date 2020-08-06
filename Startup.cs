using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using BackEnd.Models;

[assembly: OwinStartupAttribute(typeof(BackEnd.Startup))]
namespace BackEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
    public class InformationRequestHub : Hub
    {
        [HubMethodName("NotifyInformationRequestToClient")]

        public static void NotifyInformationRequestToClient()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<InformationRequestHub>();
            context.Clients.All.updatedClients();
        }

        public static void NotifyUpdatedInformationRequestToClient(DbDataPoint request)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<InformationRequestHub>();
            context.Clients.All.updatedModifyInformationRequestClients(request);
        }
    }
}
