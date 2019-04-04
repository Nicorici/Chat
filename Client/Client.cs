using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using Components;

namespace Server
{
    public class Client
    {
        private TcpClient tcpClient;
        private ChatStream chatStream;
        private string message;
        private string name = "";

        public Client()
        {
            tcpClient = new TcpClient();
            SetName();
        }

        public void Connect(IPEndPoint endpoint)
        {
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
            chatStream = new ChatStream(tcpClient.GetStream());
            Read();
            Write(" has connected.");
        }

        private void Read()
        {
            chatStream.BeginReadMessage(m=>PrintMessage(m));
        }

        private void Write(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                Write(Console.ReadLine());
            chatStream.Write((name + " : " + message),null);
            Write(Console.ReadLine());
        }

        public void Close()
        {
            chatStream.Close();
            tcpClient.Dispose();
            tcpClient.Close();
        }

        private void SetName()
        {
            Console.Write("Please input your name (1-20 characters,no \":\" characters) : ");
            bool isInvalid = true;
            while (isInvalid)
            {
                name = Console.ReadLine();
                if (name.Length < 1 || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("The name does not contain any characteres...Please input your name again : ");
                    continue;
                }
                if (name.Length > 20)
                {
                    Console.WriteLine("The name is too long...Please input your name again : ");
                    continue;
                }
                if(name.Contains(':'))
                {
                    Console.WriteLine("The name cannot contain a the \":\" character...Please input again :");
                    continue;
                }
                isInvalid = false;
            }
        }

        public void PrintMessage(Message message)
        {
            Console.WriteLine(message);
        }
    }
}
