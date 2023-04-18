using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatServer;
using DataClasses;
using MySql.Data.MySqlClient;

namespace MyProject;
class Server
{
    private static ServerContext context = new ServerContext();
    static async Task Main()
   {
        IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);
        try
        {
            using (Socket listener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                listener.Bind(ipEndPoint);
                listener.Listen(100);
                while (true)
                {
                    newConnected = await listener.AcceptAsync();
                    _ = ThreadPool.QueueUserWorkItem(new WaitCallback(Client));

                }
            }
        } catch (Exception ignored) { }
    }
    static Socket newConnected;
    static async void Client(object? state)
    {
        Socket handler = newConnected;
        while (handler.Connected)
        {
            // Receive message.
            byte[] buffer = new byte[1_024];
            int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
            string response = Encoding.UTF8.GetString(buffer, 0, received);
            if (response == null || response == "")
            {
                Console.WriteLine("Closing");
                return;
            }
            processPacket(response, handler);
        }
    }

    static void processPacket(string message, Socket handler)
    {
        Console.WriteLine(message);
        string messageType = message.Split(':')[0];
        message = message[(message.IndexOf(':') + 1)..];
        switch (messageType)
        {
            case "Logging":
                processLogging(message, handler);
                break;
            case "Register":
                processRegister(message, handler);
                break;
            case "Message":
                processMessage(message, handler);
                break;
            case "Group":
                processGroup(message, handler);
                break;
            case "User":
                ProcessUser(message, handler);
                break;
            default:
                Console.WriteLine("Unknown message type");
                SendIgnored(handler);
                break;
        }
    } 

    static async void processLogging(string message, Socket handler)
    {
        User? user = PacketHandler.DecodeUserPacket(message);
        if (user == null)
        {
            
            return;
        }

        DbSqlQuery<User> userQuery = context.Users.SqlQuery("Select * from chatapp.users where Username = @username and Password = @password", new MySqlParameter("@username", user.Username), new MySqlParameter("@password", user.Password));
        User? userReturned = null;
        foreach (User usr in userQuery)
        {
            userReturned = usr;
        }

        if (userReturned == null)
        {
            SendIgnored(handler);
            return;
        }

        if (!user.Password.Equals(userReturned.Password))
        {
            SendIgnored(handler);
            return;
        }

        userReturned = context.Users.Include("Groups").First(u => u.Id == userReturned.Id);
        

        while (true)
        {
            string information = PacketHandler.EncodeUserPacket(userReturned);
            Console.WriteLine(information);
            byte[] echoBytes = Encoding.UTF8.GetBytes(information);
            _ = await handler.SendAsync(echoBytes, 0);
            break;
        }
    }

    static async void processRegister(string message, Socket handler)
    {
        User? user = PacketHandler.DecodeUserPacket(message);
        if (user == null)
        {
            SendIgnored(handler);
            return;
        }

        DbSqlQuery<User> userQuery = context.Users.SqlQuery("Select * from chatapp.users where Username = @username", new MySqlParameter("@username", user.Username));
        User? userReturned = null;
        foreach(User usr in userQuery)
        {
            userReturned = usr;
        }
        
        if (userReturned != null)
        {
            SendIgnored(handler);
            return;
        }

        context.Users.Add(user);
        _ = await context.SaveChangesAsync();
        userQuery = context.Users.SqlQuery("Select * from chatapp.users where Username = @username", new MySqlParameter("@username", user.Username));
        foreach (User usr in userQuery)
        {
            userReturned = usr;
        }

        while (true)
        {
            string information = PacketHandler.EncodeUserPacket(userReturned);
            byte[] echoBytes = Encoding.UTF8.GetBytes(information);
            _ = await handler.SendAsync(echoBytes, 0);
            break;
        }
    }

    static async void processGroup(string message, Socket handler)
    {
        string messageType = message.Split(':')[0];
        message = message[(message.IndexOf(':') + 1)..];
        Group? group = PacketHandler.DecodeGroupPacket(message);
        if (group == null)
        {
            SendIgnored(handler);
            return;
        } 

        switch (messageType)
        {
            case "Add":
                
                if (group.Members.Count < 2)
                {
                    SendIgnored(handler);
                    return;
                }
                Group newGroup = new();
                newGroup.GroupName = group.GroupName;
                newGroup = context.Groups.Add(newGroup);
                _ = await context.SaveChangesAsync();
                foreach (User usr in group.Members)
                {
                    User temp = context.Users.Find(usr.Id);
                    newGroup.Members.Add(temp);
                }
                _ = await context.SaveChangesAsync();

                while (true)
                {
                    string information = PacketHandler.EncodeGroupPacket(newGroup);
                    byte[] echoBytes = Encoding.UTF8.GetBytes(information);
                    _ = await handler.SendAsync(echoBytes, 0);
                    break;
                }
                break;
            case "Get":
                Group? groupToGive = PacketHandler.DecodeGroupPacket(message);
                if (groupToGive == null)
                {
                    SendIgnored(handler);
                    return;
                }
                groupToGive = context.Groups.Include("Messages").Include("Members").First(g => g.Id == groupToGive.Id);
                while (true)
                {
                    string information = PacketHandler.EncodeGroupPacket(groupToGive);
                    Console.WriteLine(information);
                    byte[] echoBytes = Encoding.UTF8.GetBytes(information);
                    _ = await handler.SendAsync(echoBytes, 0);
                    break;
                }
                break;
        }
    }

    static async void processMessage(string message, Socket handler)
    {
        string messageType = message.Split(':')[0];
        message = message[(message.IndexOf(':') + 1)..];
        Group? group = PacketHandler.DecodeGroupPacket(message.Split('+')[0]);
        if (group == null)
        {
            SendIgnored(handler);
            return;
        }
        group = context.Groups.Find(group.Id);
        switch (messageType)
        {
            case "Send:":
                while (true)
                {
                    string response = message.Split('+')[1];
                    Console.WriteLine(response + "BS");
                    Message? mes = PacketHandler.DecodeMessagePacket(response);
                    if (mes == null)
                    {
                        SendIgnored(handler);
                        break;
                    }
                    mes.Sender = context.Users.Find(mes.Sender.Id);
                    mes = context.Messages.Add(mes);
                    group.Messages.Add(mes);
                    _ = await context.SaveChangesAsync();
                    string information = PacketHandler.EncodeGroupPacket(group);
                    byte[] echoBytes = Encoding.UTF8.GetBytes(information);
                    _ = await handler.SendAsync(echoBytes, 0);
                    break;
                }
                break;
        }
    }

    static async void ProcessUser(string message, Socket handler)
    {
        string messageType = message.Split(':')[0];
        message = message[(message.IndexOf(':') + 1)..];
        User? user = PacketHandler.DecodeUserPacket(message);
        if (user == null)
        {
            SendIgnored(handler);
            return;
        }

        switch (messageType)
        {
            case "Get":
            {
                DbSqlQuery<User> userQuery = context.Users.SqlQuery("Select * from chatapp.users where Username = @username", new MySqlParameter("@username", user.Username));
                User userReturned = null;
                foreach (User usr in userQuery)
                {
                    userReturned = usr;
                }

                while (true)
                {
                    string information = PacketHandler.EncodeUserPacket(userReturned);
                    byte[] echoBytes = Encoding.UTF8.GetBytes(information);
                    _ = await handler.SendAsync(echoBytes, 0);
                    break;
                }
            }
            break;
        }
    }

    static async void SendIgnored(Socket handler)
    {
        Console.WriteLine("Cancelled");
        byte[] echoBytes = Encoding.UTF8.GetBytes("no");
        _ = await handler.SendAsync(echoBytes, 0);
    }
}