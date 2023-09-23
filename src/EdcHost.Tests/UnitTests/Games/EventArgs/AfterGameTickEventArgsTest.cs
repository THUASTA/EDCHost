using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class AfterGameTickEventArgsTest
{
    [Fact]
    public void AfterGameTickEventArgs_CorrectlyInitialized()
    {
        var gameMock = new Mock<IGame>();
        int expTick = 0;
        var args = new AfterGameStartEventArgs(gameMock.Object, expTick);
        Assert.NotNull(args);
        Assert.Equal(gameMock.Object, args.Game);
        Assert.Equal(expTick, args.CurrentTick);
    }
}
