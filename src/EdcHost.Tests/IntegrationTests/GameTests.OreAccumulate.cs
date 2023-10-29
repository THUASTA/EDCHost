using EdcHost.Games;
using EdcHost.Tests.UnitTests.Games;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class GameTests
{
    const int WoolCount = 8;
    const int MaximumItemCount = 64;
    public class MockPosition : IPosition<int>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    [Fact]
    public void Test1()
    {
    Tuple<int, int> iron1 = Tuple.Create(1, 1);
    Tuple<int, int> iron2 = Tuple.Create(6, 6);
    Tuple<int, int> gold1 = Tuple.Create(3, 3);
    Tuple<int, int> gold2 = Tuple.Create(4, 4);
    Tuple<int, int> diamond1 = Tuple.Create(5, 5);
    List<Tuple<int, int>>? iron = new();
    List<Tuple<int, int>>? gold = new();
    List<Tuple<int, int>>? diamond = new();
    iron.Add(iron1);
    iron.Add(iron2);
    gold.Add(gold1);
    gold.Add(gold2);
    diamond.Add(diamond1);
    var game = IGame.Create(iron, gold, diamond);
    game.Start();

    //Act1 Increase the height of bed,1~3
    IPosition<int> position = new MockPosition { X = 0, Y = 0 };
    for(int i = 1; i < 3; i++)
    {
        game.Players[0].Place(game.Players[0].SpawnPoint.X, game.Players[0].SpawnPoint.Y);
        game.Tick();
        Assert.StrictEqual(i+1, game.GameMap.GetChunkAt(position).Height);
        Assert.StrictEqual(WoolCount - i *2, game.Players[0].WoolCount);
    }

    //Act2 Pave the way to accumulate iron 4~7
    //Invalid Placement
    game.Players[0].Place(2.4f, 2.4f);
    IPosition<int> position0 = new MockPosition { X = 2, Y = 2 };
    Assert.StrictEqual(0, game.GameMap.GetChunkAt(position0).Height);
    Assert.StrictEqual(2, game.Players[0].WoolCount);
    //Valid Placement
    IPosition<int> position1 = new MockPosition { X = 0, Y = 1 };
    Assert.StrictEqual(0, game.GameMap.GetChunkAt(position1).Height);
    game.Players[0].Place(1.4f, 0.4f);
    game.Tick();
    Assert.StrictEqual(1, game.GameMap.GetChunkAt(position1).Height);
    game.Players[0].Move(1.4f, 0.4f);
    game.Tick();
    IPosition<int> position2 = new MockPosition { X = 1, Y = 1 };
    Assert.StrictEqual(0,  game.GameMap.GetChunkAt(position2).Height);
    game.Players[0].Place(1.4f, 1.4f);
    game.Tick();
    Assert.StrictEqual(1, game.GameMap.GetChunkAt(position2).Height);
    game.Players[0].Place(1.4f, 1.4f);
    game.Players[0].Move(1.4f, 1.4f);
    game.Tick();
    Assert.StrictEqual(1, game.GameMap.GetChunkAt(position2).Height);
    Assert.StrictEqual(0,game.Players[0].WoolCount);

    // Act3 Accumulate iron 7~1607
    Assert.StrictEqual(0,game.Players[0].EmeraldCount);
    for(int i = 1; i < 200; i++)
    {
        game.Tick();
    }
    Assert.StrictEqual(0, game.Players[0].EmeraldCount);
    for(int i = 201; i < 1600; i++)
    {
        game.Tick();
    }
    Assert.StrictEqual(4, game.Players[0].EmeraldCount);
    for(int i = 1;i < 5; i++)
    {
        game.Players[0].Trade(IPlayer.CommodityKindType.Wool);
        game.Tick();
        Assert.StrictEqual(4 - i, game.Players[0].EmeraldCount);
        Assert.StrictEqual(i, game.Players[0].WoolCount);
    }
    Assert.False(game.Players[0].Trade(IPlayer.CommodityKindType.Wool));

    //Act4 Pave way and Accumlate all Ore
    game.Players[0].Place(2.4f, 1.4f);
    game.Tick();
    game.Players[0].Move(2.4f, 1.4f);
    game.Tick();
    game.Players[0].Place(3.4f, 1.4f);
    game.Tick();
    game.Players[0].Move(3.4f, 1.4f);
    game.Tick();
    game.Players[0].Place(3.4f, 2.4f);
    game.Tick();
    game.Players[0].Move(3.4f, 2.4f);
    game.Tick();
    game.Players[0].Place(3.4f, 3.4f);
    game.Tick();
    game.Players[0].Move(3.4f, 3.4f);
    game.Tick();
    Assert.StrictEqual(0, game.Players[0].WoolCount);
    Assert.StrictEqual(16, game.Players[0].EmeraldCount);
    for(int i = 1; i < 9; i++)
    {
        game.Players[0].Trade(IPlayer.CommodityKindType.Wool);
        game.Tick();
        Assert.StrictEqual(i, game.Players[0].WoolCount);
    }
    Assert.StrictEqual(0, game.Players[0].EmeraldCount);
    game.Players[0].Place(4.4f, 3.4f);
    game.Tick();
    game.Players[0].Move(4.4f, 3.4f);
    game.Tick();
    game.Players[0].Place(5.4f, 3.4f);
    game.Tick();
    game.Players[0].Move(5.4f, 3.4f);
    game.Tick();
    game.Players[0].Place(5.4f, 4.4f);
    game.Tick();
    game.Players[0].Move(5.4f, 4.4f);
    game.Tick();
    game.Players[0].Place(5.4f, 5.4f);
    game.Tick();
    game.Players[0].Move(5.4f, 5.4f);
    game.Tick();
    Assert.StrictEqual(4, game.Players[0].WoolCount);
    Assert.StrictEqual(64, game.Players[0].EmeraldCount);
    for(int i = 5; i <= MaximumItemCount; i++)
    {
        game.Players[0].Trade(IPlayer.CommodityKindType.Wool);
        game.Tick();
    }
    Assert.False(game.Players[0].Trade(IPlayer.CommodityKindType.Wool));
    game.End();
    }

}
