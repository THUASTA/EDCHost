using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;
using Xunit.Abstractions;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;

public class MessageTests
{
    private const string MessageTypeTest = "testType";
    private readonly ITestOutputHelper output;
    public MessageTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    [Fact]
    public void Message_DoNothing_ReturnsContructorValues()
    {
        IMessage message = new Message(MessageTypeTest);
        Assert.Equal(MessageTypeTest, message.MessageType);
    }
    [Fact]
    public void SerializeToUtf8Bytes_CheckEqual()
    {
        IMessage message = new Message(MessageTypeTest);
        byte[] TmpRes = message.SerializeToUtf8Bytes();
        using (MemoryStream ms = new MemoryStream(TmpRes))
        {
            object? object_tmp = JsonSerializer.Deserialize(ms, typeof(Message));
            Assert.NotNull(object_tmp);
            IMessage deserializedObject = (Message)object_tmp;
            Assert.Equivalent(message, deserializedObject);
        }
    }
    [Fact]
    public void SerializeToString_CheckEqual()
    {
        IMessage message = new Message(MessageTypeTest);
        string TmpRes = message.SerializeToString();
        object? object_tmp = JsonSerializer.Deserialize(TmpRes, typeof(Message));
        Assert.NotNull(object_tmp);
        IMessage deserializedObject = (Message)object_tmp;
        Assert.Equivalent(message, deserializedObject);
    }
}
