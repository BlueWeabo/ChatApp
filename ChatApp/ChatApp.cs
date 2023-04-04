using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;
using DataClasses;
using System.Runtime.CompilerServices;

namespace ChatApp
{
    
    public partial class ChatApp : Form
    {
        User? user;
        Group? selectedGroup;
        
        public ChatApp(User? user)
        {
            InitializeComponent();

            this.user = user;
            LoadUserGroups();
        }

        private void LoadUserGroups()
        {
            if (user == null)
            {
                return;
            }

            SuspendLayout();
            GroupsList.Controls.Clear();
            for (int i = 0; i < user.Groups.Count; i++)
            {
                GroupButton button = new();
                button.SetGroup(user.Groups.ElementAt(i));
                button.Width = GroupsList.ClientSize.Width;
                button.Height = 20;
                button.Location = new(GroupsList.Location.X, 20 * i + GroupsList.Location.Y);
                button.Click += new EventHandler(SelectGroup);
                GroupsList.Controls.Add(button);
            }
            ResumeLayout(false);

        }

        private async void SendMessage_Click(object sender, EventArgs e)
        {
            while (true)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend.Text);
                _ = await App.client.SendAsync(messageBytes, SocketFlags.None);


                // Receive ack.
                byte[] buffer = new byte[1_024];
                int received = await App.client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                break;
            }
        }
        private void ChatApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.client.Shutdown(SocketShutdown.Send);
            App.login.Close();
        }

        private void AddGroupButton_Click(object sender, EventArgs e)
        {

        }

        private void SelectGroup(object? sender, EventArgs? e)
        {
            if (sender is not GroupButton)
            {
                return;
            }

            GroupButton btn = (GroupButton)sender;

            selectedGroup = btn.GetGroup();
            LoadGroupMessages();
        }

        private void LoadGroupMessages()
        {
            if (selectedGroup == null)
            {
                return;
            }
        }
    }
}