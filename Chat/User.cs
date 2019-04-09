using System;
using System.Net.Sockets;
using Components;

namespace Server
{
    public class User
    {
        private TcpClient client;
        public ChatStream stream;
        public string Name { get; internal set; }

        public User(TcpClient client)
        {
            this.client = client;
            this.stream = new ChatStream(client.GetStream());
        }

        public void Send(Message message,Action messageSent,Action disconnect)
        {
            stream.Write(message.ToString(),null,disconnect);
        }

        public void BeginReceive(Action<Message> receive,Action readAgain=null, Action disconnect=null)
        {
            stream.BeginReadMessage(receive,readAgain, disconnect);
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
