using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;

namespace ChatApp
{
    
    public partial class ChatApp : Form
    {
        
        public ChatApp()
        {
            InitializeComponent();
        }

        private async void SendMessage_Click(object sender, EventArgs e)
        {
            while (true)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(textBox1.Text);
                _ = await App.client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await App.client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                textBox2.Text = response;
                break;
            }
        }
        private void ChatApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.client.Shutdown(SocketShutdown.Both);
        }
    }
}