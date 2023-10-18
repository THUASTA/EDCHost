using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class GamesTests
{
    [Fact]
    public void StartGame_EndGame()
    {
        IGame.Stage? stage = null;

        // Arrange: Create a game.
        var game = new Game();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Ready, stage);

        // Act: Start the game.
        game.Start();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Running, stage);

        // Act: Stop the game.
        game.Stop();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Finished, stage);
    }
}
