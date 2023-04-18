using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;
using DataClasses;
using System.Runtime.CompilerServices;
using Message = DataClasses.Message;

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
                button.Height = 40;
                button.Location = new(GroupsList.Location.X, 40 * i + GroupsList.Location.Y);
                button.Click += SelectGroup;
                GroupsList.Controls.Add(button);
            }
            ResumeLayout(false);

        }

        private async void SendMessage_Click(object sender, EventArgs e)
        {
            while (true)
            {
                string messageGroup = PacketHandler.EncodeMessageSendPacket(selectedGroup);
                Message mes = new();
                mes.Sender = user;
                mes.Text = messageToSend.Text;
                string message = PacketHandler.EncodeMessagePacket(mes);
                byte[] messageGroupBytes = Encoding.UTF8.GetBytes(messageGroup + "+" + message);
                _ = await App.client.SendAsync(messageGroupBytes, SocketFlags.None);

                byte[] buffer = new byte[1_024];
                int received = await App.client.ReceiveAsync(buffer, SocketFlags.None);
                string response = Encoding.UTF8.GetString(buffer, 0, received);
                Group? group = PacketHandler.DecodeGroupPacket(response);
                selectedGroup.Members = group.Members;
                selectedGroup.Messages = group.Messages;
                LoadGroupMessages();
                break;
            }
        }
        private void ChatApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.client.Shutdown(SocketShutdown.Send);
            App.login.Close();
        }

        private async void AddGroupButton_Click(object sender, EventArgs e)
        {
            AddFriend friendForm = new();
            if (DialogResult.OK == friendForm.ShowDialog())
            {
                while (true)
                {
                    Group newGroup = new();
                    newGroup.Members.Add(user);
                    User friendTemp = new();
                    friendTemp.Username = friendForm.FriendName.Text;
                    string friendMessage = PacketHandler.EncodeUserGetPacket(friendTemp);
                    byte[] friendBytes = Encoding.UTF8.GetBytes(friendMessage);
                    _ = await App.client.SendAsync(friendBytes, SocketFlags.None);
                    byte[] friendBuffer = new byte[1_024];
                    int friendRecieved = await App.client.ReceiveAsync(friendBuffer, SocketFlags.None);
                    string friendResponse = Encoding.UTF8.GetString(friendBuffer, 0, friendRecieved);
                    User friend = PacketHandler.DecodeUserPacket(friendResponse);
                    newGroup.Members.Add(friend);
                    newGroup.GroupName = user.Username + " Group";
                    string groupMessage = PacketHandler.EncodeGroupAddPacket(newGroup);
                    byte[] groupBytes = Encoding.UTF8.GetBytes(groupMessage);
                    _ = await App.client.SendAsync(groupBytes, SocketFlags.None);

                    byte[] groupBuffer = new byte[1_024];
                    int groupRecieved = await App.client.ReceiveAsync(groupBuffer, SocketFlags.None);
                    string groupResponse = Encoding.UTF8.GetString(groupBuffer, 0, groupRecieved);
                    Group group = PacketHandler.DecodeGroupPacket(groupResponse);
                    user.Groups.Add(group);
                    selectedGroup = group;
                    LoadGroupMessages();
                    break;
                }
                LoadUserGroups();
            }
        }

        private async void SelectGroup(object? sender, EventArgs? e)
        {
            if (sender is not GroupButton)
            {
                return;
            }

            GroupButton btn = (GroupButton)sender;
            while (true)
            {
                byte[] messageGroupBytes = Encoding.UTF8.GetBytes(PacketHandler.EncodeGroupGetPacket(btn.GetGroup()));
                _ = await App.client.SendAsync(messageGroupBytes, SocketFlags.None);

                byte[] groupBuffer = new byte[1_024];
                int groupRecieved = await App.client.ReceiveAsync(groupBuffer, SocketFlags.None);
                string groupResponse = Encoding.UTF8.GetString(groupBuffer, 0, groupRecieved);
                Group group = PacketHandler.DecodeGroupPacket(groupResponse);
                selectedGroup = group;
                break;
            }
            LoadGroup();
        }

        private void LoadGroupMessages()
        {
            if (selectedGroup == null)
            {
                return;
            }

            SuspendLayout();
            MessageList.Controls.Clear();
            for (int i = 0; i < selectedGroup.Messages.Count; i++)
            {
                Label label = new();
                label.Width = MessageList.ClientSize.Width;
                label.Height = 20;
                label.Location = new(MessageList.Location.X, 30 * i);
                MessageList.Controls.Add(label);
            }
            ResumeLayout(false);
        }

        private void LoadGroup()
        {
            if (selectedGroup == null)
            {
                return;
            }
            LoadGroupMessages();
        }
    }
}