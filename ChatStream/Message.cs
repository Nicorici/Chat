using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Components
{
    public class Message
    {
        private string message;
        public Message(byte[] message)
            : this(Encoding.ASCII.GetString(message))
        { }

        public Message(string message)
        {
            this.message = message;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;
            return true;
        }

        public int Length()
        {
            return message.Length;
        }

        public override string ToString() => message;

        public override bool Equals(object obj) => obj.ToString() == this.ToString();

        public byte[] ToByteArray() => Encoding.ASCII.GetBytes(message);

    }
}


