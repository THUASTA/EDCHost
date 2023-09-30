using EdcHost.SlaveServers.EventArgs;
using Xunit;

namespace EdcHost.Tests.UnitTests.SlaveServers.EventArgs;

public class PlayerTryAttackEventArgsTests
{
    public void PlayerTryAttackEventArgs_CorrectlyIntialized()
    {
        int expectedID = 2022;
        int expectedChunk =  32;
        PlayerTryAttackEventArgs args = new PlayerTryAttackEventArgs(2022, 32);
        Assert.Equal(expectedID, args.PlayerId);
        Assert.Equal(expectedChunk, args.TargetChunk);
        Assert.Equal("PLAYER_TRY_Attack", args.EventType);
    }
}
