using System.Text;

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

        public int Length() => message.Length;

        public override string ToString() => message;

        public override bool Equals(object obj) => obj.ToString() == this.ToString();

        public byte[] ToByteArray() => Encoding.ASCII.GetBytes(message);

        public override int GetHashCode() => base.GetHashCode();
    }
}


