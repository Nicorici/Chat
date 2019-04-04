using Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentsTests
{
    class ByteByByteStream : IStream
    {
        private string text;
        private int position;

        public ByteByByteStream(string text)
        {
            this.text = text;
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            buffer[offset] = (byte)text[position++];
            callback(null);
            return null;
        }

        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return null;
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return 1;
        }

        public void EndWrite(IAsyncResult asyncResult)
        {
        }
    }
}
