using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Chat.Server.Hubs;
using Chat.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;

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
                var client = new RestClient("http://api.icndb.com");
                var request = new RestRequest("/jokes/random?", DataFormat.Json);
                var result = await client.GetAsync<Result>(request, stoppingToken);

                _logger.LogInformation("Sending message to clients: {0} : {1}", "Chuck Fan", result.Value.Joke);
                await _chatHub.Clients.All.ReceiveMessage("Chuck Fan", result.Value.Joke);
                var nextMessageInSeconds = new Random().Next(5000, 30000);

                await Task.Delay(nextMessageInSeconds, stoppingToken);
            }
        }
    }

    [DataContract]
    public class Result
    {
        [DataMember(Name="type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public Value Value { get; set; }
    }

    [DataContract]
    public class Value
    {
        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="joke")]
        public string Joke { get; set; }
    }
}