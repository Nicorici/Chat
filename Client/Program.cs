using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
            Console.Read();
        }
    }
}
