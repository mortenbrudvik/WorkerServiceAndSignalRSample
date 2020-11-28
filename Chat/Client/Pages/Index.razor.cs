using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Client.Pages
{
    public partial class Index
    {
        protected string StatusMessage { get; set; } = "Connecting...";
        protected List<string> Messages { get; } = new List<string>();
        protected string UserInput { get; set; } = "";
        protected string MessageInput { get; set; } = "";
        
        private HubConnection _hubConnection;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .WithAutomaticReconnect() 
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                Messages.Add(encodedMsg);
                StateHasChanged();
            });

            await _hubConnection.StartAsync().ContinueWith((t) =>
            {
                StatusMessage = "Connected";
                StateHasChanged();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        protected Task Send() => _hubConnection.SendAsync("SendMessage", UserInput, MessageInput);

        protected bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }
    }
}