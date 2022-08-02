using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Server.Hubs;

public class NotificationHub : Hub
{
    public Task SendMessage(string message)
    {
        return Clients.Others.SendAsync("Send", message);
    }
}
