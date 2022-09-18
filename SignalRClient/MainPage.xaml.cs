using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    public partial class MainPage : ContentPage
    {
        private readonly HubConnection hubConnection;

        public MainPage()
        {
            InitializeComponent();

            var baseUrl = "http://localhost";
            
            // Android can't connect to localhost
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                baseUrl = "http://10.0.2.2";
            }

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}:5127/chatHub")
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                lblChat.Text += $"<b>{user}</b>: {message}<br/>";
            });

            Task.Run(() =>
            {
                Dispatcher.Dispatch(async () =>
                {
                    await hubConnection.StartAsync();
                });
            });
        }

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            await hubConnection.InvokeCoreAsync("SendMessageToAll", args: new[]
            {
                txtUsername.Text,
                txtMessage.Text
            });

            txtMessage.Text = String.Empty;
        }
    }
}