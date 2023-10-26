using EdcHost.ViewerServers;
using Moq;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class ViewerServerTests
{
    [Fact]
    public void Simple()
    {
        // Arrange
        var webSocketServerMock = new Mock<Fleck.IWebSocketServer>();

        // var updaterMock = new Mock<IUpdater>();
        var gameControllerMock = new Mock<IGameController>();

        IViewerServer viewerServer = new ViewerServer(webSocketServerMock.Object, gameControllerMock.Object);

        // Act 1
        viewerServer.Start();

        // Act 2
        viewerServer.Stop();
    }
}
