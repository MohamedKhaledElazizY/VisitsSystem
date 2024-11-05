using System.Threading.Tasks;
using VisitsSystem.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace VisitsSystem.Hubs
{
    public class NotificationHub : Hub<INotificationClient>
    {
        public async Task SendVisits(string message)
        {
            await Clients.All.ReceiveVisits(message);
        }
    }
}
