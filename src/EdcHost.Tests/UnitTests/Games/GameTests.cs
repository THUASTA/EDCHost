using EdcHost.Games;
using Moq;
using Serilog.Core;
using Serilog.Events;
using Xunit;
using System.Reflection; 

namespace EdcHost.Tests.UnitTests.Games;

public class GameTest
{
    public class MockPosition : IPosition<int>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    [Fact]
    public void Game_DoNothing_PublicMembersCorrectlyInitialized()
    {
        Game game = new Game();
        Assert.Equal(IGame.Stage.Ready, game.CurrentStage);
        Assert.Equal(TimeSpan.FromSeconds(0), game.ElapsedTime);
        Assert.Null(game.Winner);
        Assert.Equal(0, game.CurrentTick);
        MockPosition mockPosition1 = new MockPosition { X = 0, Y = 0 };
        MockPosition mockPosition2 = new MockPosition { X = 7, Y = 7 };
        var mockChunk1 = new Mock<IChunk>();
        var mockChunk2 = new Mock<IChunk>();
        mockChunk1.Setup(p => p.Height).Returns(1);
        mockChunk1.Setup(p => p.Position).Returns(mockPosition1);
        mockChunk2.Setup(p => p.Height).Returns(1);
        mockChunk2.Setup(p => p.Position).Returns(mockPosition2);
        Assert.Equal(mockChunk1.Object, game.GameMap.Chunks[0]);
        Assert.Equal(mockChunk2.Object, game.GameMap.Chunks[63]);
        Assert.NotNull(game.Players);
        Assert.Equal(0, game.Players[0].PlayerId);
        Assert.Equal(0.4f, game.Players[0].PlayerPosition.X);
        Assert.Equal(7.4f, game.Players[1].SpawnPoint.Y);
        //TODO:Mine
    }
    

    [Fact]
    public void Start_StartedYet_ThrowsCorrectException()
    {
        Game game = new Game();
        game.Start();
        Assert.Throws<InvalidOperationException>(() => game.Start());
    }

    //Todo:some members doesn't test;
    [Fact]
    public void Start_DoNothing_ReturnsCorrectValue()
    {
        Game game = new Game();
        game.Start();
        Assert.Equal(IGame.Stage.Running, game.CurrentStage);
        Assert.Equal(0, game.CurrentTick);
        Assert.Null(game.Winner);
    }

    [Fact]
    public void Start_AfterGameStartEvent_IsRaised()
    {
        bool eventReceived = false;
        Game game = new Game();
        game.AfterGameStartEvent += (sender, args) =>
        {
            eventReceived = true;
        };
        Assert.True(eventReceived);
    }
}