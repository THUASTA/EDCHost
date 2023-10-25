using Xunit;

using Moq;

using EdcHost.SlaveServers;

namespace EdcHost.Tests.IntegrationTests;

public partial class SlaveServerTests
{
    [Fact]
    public void Simple()
    {
        // Arrange
        var serialPortWrapperMock = new Mock<ISerialPortWrapper>();

        var serialPortHubMock = new Mock<ISerialPortHub>();
        serialPortHubMock.Setup(x => x.Get("COM1")).Returns(serialPortWrapperMock.Object);

        ISlaveServer slaveServer = new SlaveServer(serialPortHubMock.Object);

        // Act 1
        slaveServer.Start();

        // Act 2
        slaveServer.AddPort("COM1");

        // Act 3
        slaveServer.RemovePort("COM1");

        // Act 4
        slaveServer.Stop();
    }
}
