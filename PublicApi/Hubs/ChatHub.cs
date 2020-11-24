using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PublicApi.Interfaces;

namespace PublicApi.Hubs
{
    public class ChatHub : Hub<IClientMessage>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}