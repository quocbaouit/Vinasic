using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace VINASIC.Hubs
{
    public class RealTimeJTableDemoHub : Hub
    {
        public void SendMessage(string clientName, string message)
        {
            Clients.All.GetMessage(clientName, message);
        }

        public void SendUpdateEvent(string jtableName, string user, string message)
        {
            Clients.All.GetUpdateEvent(jtableName, user, message);
        }
    }
}