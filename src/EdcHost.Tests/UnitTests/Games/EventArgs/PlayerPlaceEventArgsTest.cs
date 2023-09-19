using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class PlayerPlaceEventArgsTests
{
    [Fact]
    public void PlayerPlaceEventArgs_CorrectlyInitialized()
    {
        var playerMock = new Mock<IPlayer>();
        var positionMock = new Mock<IPosition<float>>();
        var args = new PlayerPlaceEventArgs(playerMock.Object, positionMock.Object);
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
        Assert.Equal(positionMock.Object, args.Position);
    }
}
