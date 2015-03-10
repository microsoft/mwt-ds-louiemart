using Microsoft.AspNet.SignalR;

namespace Nop.Web.Hubs
{
    public class TraceHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.addNewMessageToPage(message);
        }
    }
}