using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class PlayerTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(int.MaxValue)]
    public void IdX_DoNothing_ReturnsConstructorValue(int X){
        int expected_MaxHealth=20;
        int expected_Strength=1;
        int expected_Initial_ActionPoints=1;
        IPlayer player=new Player(X,0,0,0,0);
        Assert.Equal(X,player.PlayerId);
        Assert.Equal(0,player.EmeraldCount);
        Assert.True(player.IsAlive);
        Assert.True(player.HasBed);
        Assert.Equal(0,player.SpawnPoint.X);
        Assert.Equal(0,player.SpawnPoint.Y);
        Assert.Equal(0,player.PlayerPosition.X);
        Assert.Equal(0,player.PlayerPosition.Y);
        Assert.Equal(0,player.WoolCount);
        Assert.Equal(expected_MaxHealth,player.Health);
        Assert.Equal(expected_MaxHealth,player.MaxHealth);
        Assert.Equal(expected_Strength,player.Strength);
        Assert.Equal(expected_Initial_ActionPoints,player.ActionPoints);
    }
    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5,0.5)]
    [InlineData(-0.5,-0.5)]
    [InlineData(0.5,-0.5)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void SpawnPoint_DoNothing_ReturnsConstructorValue(float X,float Y){
        IPlayer player=new Player(1,X,Y,0,0);
        Assert.Equal(X,player.SpawnPoint.X);
        Assert.Equal(Y,player.SpawnPoint.Y);
    }

    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5,0.5)]
    [InlineData(-0.5,-0.5)]
    [InlineData(0.5,-0.5)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void PlayerPosition_DoNothing_ReturnsConstructorValue(float X,float Y){
        IPlayer player=new Player(1,0,0,X,Y);
        Assert.Equal(X,player.PlayerPosition.X);
        Assert.Equal(Y,player.PlayerPosition.Y);
    }
}
