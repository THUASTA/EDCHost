using EdcHost.ViewerServers.EventArgs;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.EventArgs;
public class SetCameraEventArgsTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange
        const int expectedPlayerId = 1;
        object expectedCameraConfiguration = new { arg1 = "test_arg1", arg2 = "test_arg2" };

        // Act
        SetCameraEventArgs args = new SetCameraEventArgs(expectedPlayerId, expectedCameraConfiguration);

        // Assert
        Assert.Equal(expectedPlayerId, args.PlayerId);
        Assert.Same(expectedCameraConfiguration, args.CameraConfiguration);
    }
}
