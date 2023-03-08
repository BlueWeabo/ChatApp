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

        private async void ServerBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            string message = "";
            if (e.Argument is object[])
            {
                object[] arguments = (object[])e.Argument;
                if (arguments.Length == 1 && arguments[0] is string)
                {
                    message = (string)arguments[0];
                }
            }
            else
            {
                return;
            }
            while (true)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                e.Result = new object[] { response };
                break;
            }
            
            
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            ServerBackground.RunWorkerAsync(new object[] { textBox1.Text });
        }

        private void ServerBackground_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is object[] result)
            {
                if (result.Length == 1 && result[0] is string message)
                {
                    textBox2.Text = message;
                }
            }
        }

        private void ChatApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            ServerBackground.CancelAsync();
            client.Disconnect(false);
        }
    }
}