using System;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using System.Net.Sockets;


namespace Components
{
    public class ChatStream
    {
        private NetworkStream stream;
        private string pendingText = "";

        //public ChatStream(Stream stream)
        //    : this(new StreamWrapper(stream))
        //{
        //}

        public ChatStream(NetworkStream stream)
        {
            this.stream = stream;
        }

        public void BeginReadMessage(Action<Message> readComplete, Action disconnect = null)
        {
            var index = pendingText.IndexOf('\0');
            if (index != -1)
            {
                readComplete(new Message(pendingText.Substring(0, index)));
                pendingText = pendingText.Substring(index+1);
                return;
            }

            byte[] buffer = new byte[1024];
            stream.BeginRead(buffer, 0, buffer.Length, r =>
            {
                int read = 0;
                try
                {
                    read = stream.EndRead(r);
                    if (read == 0)
                    {
                        Console.WriteLine("Disconnect in read{0}",read);
                        throw new Exception();
                    }
                }
                catch 
                {
                    Console.WriteLine("diconecet catch.");
                    disconnect?.Invoke();
                    return;
                }

                var stringRead = Encoding.UTF8.GetString(buffer, 0, read);
                pendingText += stringRead;
                index = pendingText.IndexOf('\0');

                if (index != -1)
                {
                    readComplete(new Message(pendingText.Substring(0, index)));
                    pendingText = pendingText.Substring(index + 1);
                }
                //else
                //{
                //    BeginReadMessage(readComplete, disconnect);
                //}
                BeginReadMessage(readComplete, disconnect);
            }, null);
        }

        public void Write(string text , Action writeComplete,Action exception=null)
        {
            Message message = new Message(text + "\0");
            stream.BeginWrite(message.ToByteArray(), 0, message.Length(), m =>
            {
                try
                {
                    stream.EndWrite(m);
                    writeComplete?.Invoke();
                }
                catch 
                {
                    exception?.Invoke();
                    return;
                }
            }, null);
        }

        public void Close()
        {
            stream.Flush();
            stream.Close();
        }

    }
}
