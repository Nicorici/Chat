using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chat
{
    public class Client
    {
        public string Name { get; }
        public TcpClient TcpClient { get; set; }
        public bool IsConnected { get => TcpClient.Connected; }

        public Client()
        {
            Console.Write("Please input your name : ");
            Name = Console.ReadLine();
            TcpClient = new TcpClient();
        }

        public void Connect(IPEndPoint endpoint)
        {
            try
            {
                TcpClient.Connect(endpoint);
                SendReceiveData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public void Close()
        {
            TcpClient.Dispose();
            TcpClient.Close();
        }

        private void SendReceiveData()
        {
            if (IsConnected)
            {
                var stream = TcpClient.GetStream();
                byte[] buffer = Encoding.ASCII.GetBytes($"{Name} has connected."+'\0');
                stream.Write(buffer);

                ReadStream(stream, buffer, "");
                WriteStream(stream, buffer);
            }
        }

        static void Main(string[] args)
        {
            var client = new Client();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
            Console.Read();
        }

        private  void WriteStream(NetworkStream stream, byte[] buffer)
        {
            buffer = Encoding.ASCII.GetBytes( Name + Console.ReadLine()+'\0');
            stream.BeginWrite(buffer, 0, buffer.Length, r =>
            {
                stream.EndWrite(r);
                WriteStream(stream, buffer);
            }, null);

        }

        private static void ReadStream(NetworkStream stream, byte[] buffer, string message)
        {
            stream.BeginRead(buffer,
                0,
                buffer.Length, r =>
                {
                    var read = stream.EndRead(r);
                    if (read == 0)
                    {
                        Console.WriteLine("Server has disconnected.");
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
                                message = "";
                                break;
                            }
                        }
                    ReadStream(stream, buffer, message);
                },
                null);
        }
    }
}
