using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class AfterJudgementEventArgsTest
{
    [Fact]
    public void AfterJudgementEventArgs_CorrectlyInitialized()
    {
        var gameMock = new Mock<IGame>();
        var playerMock = new Mock<IPlayer>();
        var args = new AfterJudgementEventArgs(gameMock.Object, playerMock.Object);
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
        Assert.Equal(gameMock.Object, args.Game);
    }
}