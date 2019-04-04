using System;
using System.Text;
using Components;

namespace ComponentsTests
{
    internal class AllAtOnceStream: IStream
    {
        private byte[] text;
        private bool isDone;

        public AllAtOnceStream(string text)
        {
            this.text = Encoding.UTF8.GetBytes(text);
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (isDone)
                throw new Exception("Not allowed");

            Array.Copy(text, buffer, text.Length);
            callback(null);
            isDone = true;
            return null;
        }

        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return null;
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return text.Length;
        }

        public void EndWrite(IAsyncResult asyncResult)
        {

        }
    }
}