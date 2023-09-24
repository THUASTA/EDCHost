using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.EventArgs;

public class MessageTransferEventArgsTests
{
    [Fact]
    public void MessageTransferEventArgs_DoNothing_ReturnsContructorValue()
    {
        string messageTypeTest = "testType";
        var messageMock = new Mock<IMessage>();
        var messageTransferEvent = new MessageTransferEventArgs(messageMock.Object);
        Assert.Equal(messageTypeTest, messageTransferEvent.Message.MessageType);
    }
}
