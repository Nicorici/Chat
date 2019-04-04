using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    public interface IStream
    {
        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state);
        int EndRead(IAsyncResult asyncResult);
        void EndWrite(IAsyncResult asyncResult);
    }
}
