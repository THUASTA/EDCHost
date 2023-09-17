using Xunit;
using Moq;
using EdcHost.Games;
namespace  EdcHost.Tests.UnitTests.Games.EventArgs;

public class PlayerMoveEventArgsTests
{
    [Fact]
    public void PlayerMoveEventArgs_CorrectlyInitialized()
    {
        // Arrange
        var playerMock = new Mock<IPlayer>();
        var positionBeforeMovementMock = new Mock<IPosition<float>>();
        var positionMock = new Mock<IPosition<float>>();

        // Act
        var args = new PlayerMoveEventArgs(playerMock.Object, 
            positionBeforeMovementMock.Object, positionMock.Object);

        // Assert
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
        Assert.Equal(positionBeforeMovementMock.Object, args.PositionBeforeMovement);
        Assert.Equal(positionMock.Object, args.Position);
    }
}