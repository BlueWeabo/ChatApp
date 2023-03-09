using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;

namespace ChatApp
{
    
    public partial class ChatApp : Form
    {
        private Socket client;
        private IPEndPoint ipEndPoint;
        public ChatApp()
        {
            InitializeComponent();
            StartServer();
        }

        private async void StartServer()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("Kasi-PC");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            ipEndPoint = new(ipAddress, 11_000);
            client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(ipEndPoint);
        }

        private async void SendMessage_Click(object sender, EventArgs e)
        {
            while (true)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(textBox1.Text);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                textBox2.Text = response;
                break;
            }
        }
        private void ChatApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
            client.Disconnect(false);
            client.Shutdown(SocketShutdown.Send);
        }
    }
}