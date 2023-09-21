using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class PlayerAttackEventArgsTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void PlayerAttackEventArgs_CorrectlyInitialized(int strengthValue)
    {
        var playerMock = new Mock<IPlayer>();
        var positionMock = new Mock<IPosition<float>>();
        var args = new PlayerAttackEventArgs(playerMock.Object, strengthValue, positionMock.Object);
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
        Assert.Equal(strengthValue, args.Strength);
        Assert.Equal(positionMock.Object, args.Position);
    }
}
