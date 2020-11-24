using System.Threading.Tasks;

namespace PublicApi.Interfaces
{
    public interface IClientMessage
    {
        Task ReceiveMessage(string user, string message);
    }
}