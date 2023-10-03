using EdcHost.ViewerServers.EventArgs;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.EventArgs;
public class SetPortEventArgsTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange
        const int expectedPlayerId = 1;
        const string expectedPortName = "COM1";
        const int expectedBaudRate = 9600;

        // Act
        SetPortEventArgs args = new SetPortEventArgs(expectedPlayerId, expectedPortName, expectedBaudRate);

        // Assert
        Assert.Equal(expectedPlayerId, args.PlayerId);
        Assert.Equal(expectedPortName, args.PortName);
        Assert.Equal(expectedBaudRate, args.BaudRate);
    }
}
