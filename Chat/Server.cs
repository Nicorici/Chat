using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private ChatRoom chatRoom = new ChatRoom();
        private User user;
        private TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));

        public void Start()
        {
            listener.Start();
        }

        public void AcceptClient()
        {
            while (true)
            {
                user = new User( listener.AcceptTcpClient());
                chatRoom.Join(user);
            }
        }

        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
            Console.WriteLine("The server has started.");
            server.AcceptClient();

        }
    }
}
