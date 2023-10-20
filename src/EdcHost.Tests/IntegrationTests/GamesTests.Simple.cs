using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class GamesTests
{
    [Fact]
    public async Task Simple()
    {
        IGame.Stage? stage = null;

        // Arrange: Create a game.
        var game = new Game();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Ready, stage);

        // Act: Start the game.
        await game.Start();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Running, stage);

        // Act: Stop the game.
        await game.End();

        stage = game.CurrentStage;
        Assert.StrictEqual(IGame.Stage.Ended, stage);
    }
}
