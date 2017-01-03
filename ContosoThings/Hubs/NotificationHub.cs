using System;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace ContosoThings
{
    public class NotificationHub : Hub
    {
        public void NotifyAllClients()
        {
            Clients.All.refresh();
        }
    }
}