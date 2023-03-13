using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatServer;

namespace MyProject;
class Server
{
    static async Task Main()
    {
        IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint ipEndPoint = new(ipAddress, 11_000);
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
            Console.WriteLine(
                $"Socket server received message: \"{response}\"");
            if (response == null || response == "")
            {
                Console.WriteLine("Closing");
                return;
            }
            string ackMessage = "recieved";
            byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
            _ = await handler.SendAsync(echoBytes, 0);
            Console.WriteLine(
                $"Socket server sent acknowledgment: \"{ackMessage}\"");
        }
    }
}