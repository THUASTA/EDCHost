using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games.EventArgs;

public class AfterGameStartEventArgsTest
{
    [Fact]
    public void AfterGameStartEventArgs_CorrectlyInitialized()
    {
        var playerMock = new Mock<IPlayer>();
        DateTime? dateTime = new DateTime(2023, 10, 1, 0, 0, 0);
        var args = new AfterGameStartEventArgs(playerMock.Object, dateTime);
        Assert.NotNull(args);
        Assert.Equal(playerMock.Object, args.Player);
        Assert.Equal(dateTime, args.DateTime);
    }
}
