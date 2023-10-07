using EdcHost.ViewerServers;
using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers;
public class GameControllerTests
{
    [Fact]
    public void StartGameEvent_Invoke_ShouldRaiseStartGameEvent()
    {
        // Arrange
        var gameController = new GameController();
        bool eventRaised = false;
        gameController.StartGameEvent += (sender, e) => eventRaised = true;

        // Act
        gameController.StartGame();

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void EndGameEvent_Invoke_ShouldRaiseEndGameEvent()
    {
        // Arrange
        var gameController = new GameController();
        bool eventRaised = false;
        gameController.EndGameEvent += (sender, e) => eventRaised = true;

        // Act
        gameController.EndGame();

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void ResetGameEvent_Invoke_ShouldRaiseResetGameEvent()
    {
        // Arrange
        var gameController = new GameController();
        bool eventRaised = false;
        gameController.ResetGameEvent += (sender, e) => eventRaised = true;

        // Act
        gameController.ResetGame();

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void GetHostConfiguration_Invoke_ShouldRaiseGetHostConfigurationEvent()
    {
        // Arrange
        var gameController = new GameController();
        var availableCameras = new List<string> { "Camera1", "Camera2" };
        var availablePorts = new List<string> { "COM1", "COM2" };
        
        gameController.SetAvailableDevice(availablePorts.ToArray(), availableCameras.ToArray());
        bool eventRaised = false;

        // Act
        gameController.GetHostConfigurationEvent += (sender, e) =>
        {
            eventRaised = true;
            var args = e as MessageTransferEventArgs;
            var hostConfig = args?.Message as HostConfigurationFromServer;

            // Assert
            Assert.NotNull(args);
            Assert.NotNull(hostConfig);
            Assert.Equal(availableCameras, hostConfig.AvailableCameras);
            Assert.Equal(availablePorts, hostConfig.AvailableSerialPorts);
        };
        gameController.GetHostConfiguration();

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void GetHostConfiguration_WithoutAvailableDevices_ShouldThrowException()
    {
        // Arrange
        var gameController = new GameController();

        // Act & Assert
        Exception exception =Assert.Throws<Exception>(() => gameController.GetHostConfiguration());
        Assert.Equal("Ports or cameras are not set.",exception.Message);
    }
}
