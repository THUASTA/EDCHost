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
        Assert.Null(game.Winner);
        Assert.Equal(0, game.ElapsedTicks);
        Assert.Equal(0, game.GameMap.Chunks[0].Position.X);
        Assert.Equal(0, game.GameMap.Chunks[0].Position.Y);
        Assert.Equal(1, game.GameMap.Chunks[0].Height);
        Assert.Equal(7, game.GameMap.Chunks[63].Position.X);
        Assert.Equal(7, game.GameMap.Chunks[63].Position.Y);
        Assert.Equal(1, game.GameMap.Chunks[63].Height);
    }

    [Fact]
    public async Task Start_StartedYet_ThrowsCorrectException()
    {
        Game game = new();
        await game.Start();
        await Assert.ThrowsAsync<InvalidOperationException>(() => game.Start());
    }

    [Fact]
    public async Task Start_DoNothing_ReturnsCorrectValue()
    {
        Game game = new Game();
        await game.Start();
        Assert.Equal(0, game.Players[0].PlayerId);
        Assert.Equal(0.4f, game.Players[0].SpawnPoint.X);
        Assert.Equal(0.4f, game.Players[0].PlayerPosition.Y);
        Assert.Equal(1, game.Players[1].PlayerId);
        Assert.Equal(7.4f, game.Players[1].SpawnPoint.X);
        Assert.Equal(7.4f, game.Players[1].PlayerPosition.Y);
        Assert.Equal(IGame.Stage.Running, game.CurrentStage);
        Assert.Equal(0, game.ElapsedTicks);
        Assert.Null(game.Winner);
    }

    [Fact]
    public async Task Start_AfterGameStartEvent_IsRaised()
    {
        bool eventReceived = false;
        Game game = new Game();
        game.AfterGameStartEvent += (sender, args) =>
        {
            eventReceived = true;
        };
        await game.Start();
        Assert.True(eventReceived);
    }
}
