using System.Threading;
using System.Threading.Tasks;
using Chat.Server.Hubs;
using Chat.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Workers
{
    public class ChuckNorrisBotWorker : BackgroundService
    {
        private readonly ILogger<ChuckNorrisBotWorker> _logger;
        private readonly IHubContext<ChatHub, IClientMessage> _chatHub;

        public ChuckNorrisBotWorker(ILogger<ChuckNorrisBotWorker> logger, IHubContext<ChatHub, IClientMessage> chatHub )
        {
            _logger = logger;
            _chatHub = chatHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                const string message = "more later";
                _logger.LogInformation("Sending message to clients: {0} : {1}", "Chuck Norris", message);
                await _chatHub.Clients.All.ReceiveMessage("Chuck Norris", message);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}