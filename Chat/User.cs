using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using Components;

namespace Server
{
    public class User
    {
        private TcpClient client;
        private string name;
        public ChatStream stream;

        public string Name { get ; internal set; }

        public User(TcpClient client)
        {
            this.client = client;
            this.stream = new ChatStream(client.GetStream());
        }

        public void Send(byte[] message)
        {
            stream.Write(message.ToString(),null);
        }

        public void BeginReceive(Action<Message> receive)
        {
            stream.BeginReadMessage(s =>
            {
                receive(s);
            });
        }

        public void Close()
        {
            stream.Close();
            client.Dispose();
            client.Close();
        }

        public void PrintMessage(Message message)
        {
            Console.WriteLine(message);
        }
    }
}
