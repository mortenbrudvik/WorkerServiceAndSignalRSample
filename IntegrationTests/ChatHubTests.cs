using System.Threading.Tasks;
using Chat.Server.Hubs;
using Chat.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace IntegrationTests
{
    public class ChatHubTests
    {
        [Fact]
        public async Task SendMessage_ShouldSendMessageToClients_WhenCalled()
        {
            var clientsMock = new Mock<IHubCallerClients<IClientMessage>>();
            var clientProxyMock = new Mock<IClientMessage>();
            clientsMock.Setup(clients => clients.All).Returns(clientProxyMock.Object);
            var chatHub = new ChatHub {Clients = clientsMock.Object};

            await chatHub.SendMessage("Jack", "Hello all!");

            clientsMock.Verify(clients => clients.All, Times.Once);
            clientProxyMock.Verify(x => x.ReceiveMessage(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
