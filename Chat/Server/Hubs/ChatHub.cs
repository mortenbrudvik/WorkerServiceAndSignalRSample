using System.Threading.Tasks;
using Chat.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Server.Hubs
{
    public class ChatHub : Hub<IClientMessage>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}