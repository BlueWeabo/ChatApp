using ChatServer;

namespace MyProject;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(IPHelper.GetLocalIPAddress());
    }
}