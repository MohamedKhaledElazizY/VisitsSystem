using System.Threading.Tasks;

namespace VisitsSystem.Hubs.Clients
{
    public interface INotificationClient
    {
        Task ReceiveVisits(string message);
    }
}
