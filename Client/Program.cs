using System;
using System.Net;
using System.Collections.Generic;
using System.Threading;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
        }
    }
}
