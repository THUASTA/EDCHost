using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class PlayerTests
{
    const int Expected_MaxHealth=20;
    const int Expected_Strength=1;
    const int Expected_Initial_ActionPoints=1;
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
        Assert.Equal(Expected_MaxHealth,player.Health);
        Assert.Equal(Expected_MaxHealth,player.MaxHealth);
        Assert.Equal(Expected_Strength,player.Strength);
        Assert.Equal(Expected_Initial_ActionPoints,player.ActionPoints);
    }
    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5f,0.5f)]
    [InlineData(-0.5f,-0.5f)]
    [InlineData(0.5f,-0.5f)]
    [InlineData(1.0f/3,1.0f/3)]
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
    [InlineData(0.5f,0.5f)]
    [InlineData(-0.5f,-0.5f)]
    [InlineData(0.5f,-0.5f)]
    [InlineData(1.0f/3,1.0f/3)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void PlayerPosition_DoNothing_ReturnsConstructorValue(float X,float Y){
        IPlayer player=new Player(1,0,0,X,Y);
        Assert.Equal(X,player.PlayerPosition.X);
        Assert.Equal(Y,player.PlayerPosition.Y);
    }

    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5f,0.5f)]
    [InlineData(-0.5f,-0.5f)]
    [InlineData(0.5f,-0.5f)]
    [InlineData(1.0f/3,1.0f/3)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void Move_ToNewPosition_ReturnsNewCoordinate(float newX,float newY){
        IPlayer player=new Player();
        player.OnMove+=(this_x,args)=>{
            Assert.Equal(0,args.PositionBeforeMovement.X);
            Assert.Equal(0,args.PositionBeforeMovement.Y);
            Assert.Equal(newX,args.Position.X);
            Assert.Equal(newY,args.Position.Y);
            Assert.Equal(player,args.Player);
            Assert.Equal(player,this_x);
        };
        player.Move(newX,newY);
        Assert.Equal(newX,player.PlayerPosition.X);
        Assert.Equal(newY,player.PlayerPosition.Y);
    }

    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5f,0.5f)]
    [InlineData(-0.5f,-0.5f)]
    [InlineData(0.5f,-0.5f)]
    [InlineData(1.0f/3,1.0f/3)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void Attack_TestPosition_CheckEvent(float newX,float newY){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnAttack += (this_x,args) => {
            Assert.Equal(Expected_Strength,args.Strength);
            Assert.Equal(newX,args.Position.X);
            Assert.Equal(newY,args.Position.Y);
            Assert.Equal(player,args.Player);
            Assert.Equal(player,this_x);
            event_triggered=true;
        };
        player.Attack(newX,newY);
        Assert.True(event_triggered);
    }
    //TODO: wait for implementation of WoolCount adding.
    /*
    [Theory]
    [InlineData(0,0)]
    [InlineData(0.5f,0.5f)]
    [InlineData(-0.5f,-0.5f)]
    [InlineData(0.5f,-0.5f)]
    [InlineData(1.0f/3,1.0f/3)]
    [InlineData(float.MaxValue,float.MaxValue)]
    [InlineData(float.MinValue,float.MinValue)]
    [InlineData(float.MaxValue,float.MinValue)]
    public void Place_TestPosition_HasWool_EventTriggered(float newX,float newY){
        IPlayer player=new Player();
        //TODO: ADD the WoolCount of the player
        //
        bool event_triggered=false;
        player.OnPlace+=(this_x,args)=>{
            Assert.Equal(newX,args.Position.X);
            Assert.Equal(newY,args.Position.Y);
            Assert.Equal(player,args.Player);
            Assert.Equal(player,this_x);
            event_triggered=true;
        };
        player.Place(newX,newY);
        Assert.True(event_triggered);
    }*/
    [Fact]
    public void Place_NoWool_EventNotTriggered(){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnPlace+=(this_x,args)=>{
            event_triggered=true;
        };
        player.Place(0,0);
        Assert.False(event_triggered);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(19)]
    public void Hurt_LessthanHealth_CheckNewHealth(int EnemyStrength){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnDie+=(this_x,args)=>{
            event_triggered=true;
        };
        player.Hurt(EnemyStrength);
        Assert.Equal(Expected_MaxHealth-EnemyStrength,player.Health);
        Assert.False(event_triggered);
        Assert.True(player.IsAlive);
    }
    [Theory]
    [InlineData(20)]
    [InlineData(int.MaxValue)]
    public void Hurt_CoversHealth_PlayerDie(int EnemyStrength){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnDie+=(this_x,args)=>{
            Assert.Equal(player,this_x);
            Assert.Equal(player,args.Player);
            event_triggered=true;
        };
        player.Hurt(EnemyStrength);
        Assert.Equal(0,player.Health);
        Assert.True(event_triggered);
        Assert.False(player.IsAlive);
    }
    //TODO: TEST PerformActionPosition
    [Fact]
    public void PerformActionPosition_Attack_CheckEvent(){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnAttack += (this_x,args) => {
            event_triggered=true;
        };
        player.PerformActionPosition(IPlayer.ActionKindType.Attack, 0,0);
        Assert.True(event_triggered);
    }
    [Fact]
    public void PerformActionPosition_Place_CheckEvent(){
        IPlayer player=new Player();
        bool event_triggered=false;
        player.OnPlace += (this_x,args) => {
            event_triggered=true;
        };
        player.PerformActionPosition(IPlayer.ActionKindType.PlaceBlock, 0,0);
        Assert.False(event_triggered);
        //TODO: Add WoolCount and try again
    }
    //TODO: TEST Trade
}
