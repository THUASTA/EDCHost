using EdcHost.SlaveServers.EventArgs;
using Xunit;

namespace EdcHost.Tests.UnitTests.SlaveServers.EventArgs;

public class PlayerTryTradeEventArgsTests
{
    public void PlayerTryTradeEventArgs_CorrectlyIntialized()
    {
        int expectedID = 2022;
        int expectedItem = 32;
        PlayerTryTradeEventArgs args = new PlayerTryTradeEventArgs(2022, 32);
        Assert.Equal(expectedID, args.PlayerId);
        Assert.Equal(expectedItem, args.Item);
        Assert.Equal("PLAYER_TRY_TRADE", args.EventType);
    }
}
