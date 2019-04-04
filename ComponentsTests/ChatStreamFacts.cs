using Components;
using System;
using System.IO;
using System.Threading;
using Xunit;

namespace ComponentsTests
{
    public class ChatStreamFacts
    {
        //    [Fact]
        //    public void ReadsASimpleMessage()
        //    {
        //        var testStream = new ByteByByteStream("test\0fr\0");

        //        var stream = new ChatStream(testStream);
        //        Message message = null;
        //        Message message1 = null;
        //        stream.BeginReadMessage(m => message = m);
        //        Assert.Equal("test", message.ToString());
        //    }

        //    [Fact]
        //    public void ReadsMultipleMessagesAtOnce()
        //    {
        //        var testStream = new AllAtOnceStream("first\0second\0third\0ccccc");

        //        var stream = new ChatStream(testStream);
        //        Message first = null;
        //        Message second = null;
        //        Message third = null;
        //        Message forth = null;
        //        stream.BeginReadMessage(m => first = m);
        //        stream.BeginReadMessage(m => second = m);
        //        stream.BeginReadMessage(m => third = m);
        //        Assert.Equal("first", first.ToString());
        //        Assert.Equal("second", second.ToString());
        //        Assert.Equal("third", third.ToString());
        //    }
    }
}
