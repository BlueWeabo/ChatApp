using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataClasses;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChatApp
{
    public partial class LoginRegister : Form
    {
        public LoginRegister()
        {
            InitializeComponent();
            LoginRegisterSplit.Panel1.Show();
            LoginRegisterSplit.SplitterDistance = Width;
            LoginRegisterSplit.Panel2.Hide();
        }

        private void GoToLoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginRegisterSplit.Panel1.Show();
            LoginRegisterSplit.SplitterDistance = Width;
            LoginRegisterSplit.Panel2.Hide();
        }

        private void GoToRegisterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginRegisterSplit.Panel2.Show();
            LoginRegisterSplit.SplitterDistance = 0;
            LoginRegisterSplit.Panel1.Hide();
        }

        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            while (true)
            {
                User user = new()
                {
                    Username = RegUserTxt.Text,
                    Password = User.Sha256(RegPasswordTxt.Text)
                };
                byte[] messageBytes = Encoding.UTF8.GetBytes(PacketHandler.EncodeRegisterPacket(user));
                _ = await App.client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await App.client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                if (response == "")
                {
                    MessageBox.Show("Usename Taken");
                    return;
                }

                User? returned = PacketHandler.DecodeRegisterPacket(response.Split(':')[1]);
                new ChatApp(returned).Show();
                Close();
                break;
            }
            
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            while (true)
            {
                User user = new()
                {
                    Username = LogUserTxt.Text,
                    Password = User.Sha256(LogPasswordTxt.Text)
                };
                byte[] messageBytes = Encoding.UTF8.GetBytes(PacketHandler.EncodeLoginPacket(user));
                _ = await App.client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await App.client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                if (response == "")
                {
                    MessageBox.Show("Wrong login cridentials");
                    return;
                }

                User? returned = PacketHandler.DecodeLoginPacket(response.Split(':')[1]);
                new ChatApp(returned).Show();
                Close();
                break;
            }
        }
    }
}
