using System;
using System.Net;
using System.Net.Sockets;
using Components;

namespace Server
{
    public class Client
    {
        private TcpClient tcpClient;
        private ChatStream chatStream;
        private string name = "";
        public string Name { get => name; }

        public Client(string name)
        {
            tcpClient = new TcpClient();
            this.name = name;
            // SetName();
        }

        public void Connect(IPEndPoint endpoint)
        {
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5500));
            chatStream = new ChatStream(tcpClient.GetStream());
          //  Read();
            //Write(" has connected.");
        }

        public void Read()
        {
            chatStream.BeginReadMessage(m => PrintMessage(m), Read, () =>
             {
                 Console.WriteLine("The server is not responding...");
                 Close();
             });
        }

        public void Write(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
              //  Write(Console.ReadLine());
            }

            if (message == "DISCONNECT.")
            {
                chatStream.Write((name + " : " + message), null, Close);
                Close();
                Environment.Exit(0);
            }
            for (int i = 0; i < 5; i++)
            {
                message = $"this is the {i} message from client {name}";
                chatStream.Write((name + " : " + message ), null, Close);
            }
            //  Write(Console.ReadLine());

            //message = Console.ReadLine();
            //WriteMessage(message, 1000000);

        }

        private void WriteMessage(string message, int times)
        {
            Console.WriteLine($"sendinng message{ times}");
            chatStream.Write((name + " : " + message), () =>
            {
                if (times > 0)
                    WriteMessage(message, times - 1);
            }, null);
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
                if (name.Contains(':'))
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
