using Microsoft.AspNet.SignalR;

namespace Nop.Web.Hubs
{
    public class TraceHub : Hub
    {
        public void Send(string message, double timeStamp)
        {
            Clients.All.addNewMessageToPage(message, timeStamp);
        }
    }
}