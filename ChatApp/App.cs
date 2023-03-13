using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    internal static class App
    {
        public static Socket client;
        public static IPEndPoint ipEndPoint;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ConnectToServer();
            //Application.Run(new LoginRegister());
            Application.Run(new ChatApp());
        }

        private async static void ConnectToServer()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            ipEndPoint = new(ipAddress, 11_000);
            client = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await client.ConnectAsync(ipEndPoint);
        }
    }
}