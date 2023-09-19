using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class PlayerMoveEventArgsTests
{
    [Fact]
    public void PlayerDieEventArgs_CorrectlyInitialized()
    {
        var playerMock = new Mock<IPlayer>();
        var args = new PlayerDieEventArgs(playerMock.Object);
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
    }
}
