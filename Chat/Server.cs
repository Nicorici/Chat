using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;


namespace Chat
{
    public class Server
    {
        private static TcpClient client;
        private TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
        private static List<TcpClient> clients = new List<TcpClient>();
        private NetworkStream stream;

        static void Main(string[] args)
        {

            var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500);
            var server = new TcpListener(endpoint);
            NetworkStream stream;
            byte[] message = new byte[10];

            server.Start();
            while (true)
            {
                client = server.AcceptTcpClient();
                clients.Add(client);
                stream = client.GetStream();
                Read(stream, message, "");
            }

        }

        public void Start()
        {
            tcpListener.Start();
        }

        private void SendReceiveData(Client client)
        {
            byte[] buffer = new byte[100];
            stream = client.TcpClient.GetStream();
            string name = client.Name;
            Read(stream, buffer, "");
        }

        private static void Read(NetworkStream stream, byte[] buffer, string message)
        {
            int read = 0;
            stream.BeginRead(buffer, 0, buffer.Length, r =>
            {
                try
                {
                    read = stream.EndRead(r);
                }
                catch (Exception)
                {
                    Console.WriteLine($" has disconected");
                    clients.Remove(client);
                    stream.Dispose();
                    stream.Close();
                    return;
                }
                string temp = Encoding.ASCII.GetString(buffer);
                for (int i = 0; i < temp.Length; i++)
                    if (temp[i] != '\0')
                        message += temp[i];
                    else
                    {
                        if (message.Length > 0)
                        {
                            Console.WriteLine(message);
                            message += '\0';
                            buffer = Encoding.ASCII.GetBytes(message);
                            SendDataToAll(buffer);
                            message = "";
                            break; 
                        }
                    }
                Read(stream, buffer, message);
            }, null);

        }

        private static void SendDataToAll(byte[] message)
        {
            NetworkStream stream;
            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    stream = client.GetStream();
                    stream.Write(message);
                }
                else
                {
                    clients.Remove(client);
                }
            }
        }
    }
}
