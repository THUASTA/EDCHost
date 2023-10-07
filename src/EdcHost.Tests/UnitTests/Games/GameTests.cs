using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class GameTest
{
    [Fact]
    public void Game_DoNothing_PublicMembersCorrectlyInitialized()
    {
        Game game = new Game();
        Assert.Equal(IGame.Stage.Ready, game.CurrentStage);
        Assert.Equal(TimeSpan.FromSeconds(0), game.ElapsedTime);
        Assert.Null(game.Winner);
        Assert.Equal(0, game.CurrentTick);
        Assert.Equal(0, game.GameMap.Chunks[0].Position.X);
        Assert.Equal(0, game.GameMap.Chunks[0].Position.Y);
        Assert.Equal(1, game.GameMap.Chunks[0].Height);
        Assert.Equal(7, game.GameMap.Chunks[63].Position.X);
        Assert.Equal(7, game.GameMap.Chunks[63].Position.Y);
        Assert.Equal(1, game.GameMap.Chunks[63].Height);
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
        game.Start();
        Assert.True(eventReceived);
    }
}
