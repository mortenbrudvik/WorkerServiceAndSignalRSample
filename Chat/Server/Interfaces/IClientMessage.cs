using System.Threading.Tasks;

namespace Chat.Server.Interfaces
{
    public interface IClientMessage
    {
        Task ReceiveMessage(string user, string message);
    }
}