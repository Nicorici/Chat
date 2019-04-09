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
            var clients = new List<Client>();
            for (int i = 0; i < 5; i++)
            {
                Client client = new Client($"user {i}");
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
                clients.Add(client);
            }
            foreach (var c in clients)
            {
                c.Read();
            }

            foreach (var c in clients)
            {
                new Thread(() => c.Write(c.Name)).Start();
                
            }

            Console.Read();
        }
    }
}
