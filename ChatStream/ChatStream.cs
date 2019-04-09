using System;
using System.Text;
using System.Net.Sockets;


namespace Components
{
    public class ChatStream
    {
        private NetworkStream stream;
        private string pendingText = "";

        public ChatStream(NetworkStream stream)
        {
            this.stream = stream;
        }

        public void BeginReadMessage(Action<Message> messageComplete, Action readAgain=null, Action disconnect = null)
        {
            Message mess = new Message("");
            var index = pendingText.IndexOf('\0');
           
            byte[] buffer = new byte[1024];
            if (StreamIsOn())
                stream.BeginRead(buffer, 0, buffer.Length, r =>
                {
                    int read = 0;
                    try
                    {
                        read = stream.EndRead(r);
                    }
                    catch
                    {
                        disconnect?.Invoke();
                        return;
                    }

                    var stringRead = Encoding.UTF8.GetString(buffer, 0, read);
                    pendingText += stringRead;
                    index = pendingText.IndexOf('\0');
                  
                    while (index != -1)
                    {
                        mess = new Message(pendingText.Substring(0, index));
                        pendingText = pendingText.Substring(index + 1);
                        index = pendingText.IndexOf('\0');
                        messageComplete(mess);
                    }
                    readAgain?.Invoke();
                }, disconnect);
        }

        public void Write(string text, Action writeComplete, Action exception = null)
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
            }, exception);
        }

        private bool StreamIsOn()
        {
            return stream.CanRead && stream.CanWrite;
        }

        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
    }
}
