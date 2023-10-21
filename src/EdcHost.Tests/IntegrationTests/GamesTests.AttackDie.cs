using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class GamesTests
{
    const int AttackTickInterval = 20;
    const int AttackTimes = 20;
    [Fact]
    public void Game_AttackDieTests()
    {
        var game = IGame.Create();
        game.Start();
        game.Players[0].Move(game.Players[1].PlayerPosition.X, game.Players[1].PlayerPosition.Y);
        game.Tick();

        // Accumulate no ore and attack 20 times
        for (int i = 1; i <= AttackTimes; i++)
        {
            game.Players[0].Attack(game.Players[1].PlayerPosition.X, game.Players[1].PlayerPosition.Y);
            game.Tick();
            //Assert.StrictEqual(game.Players[1].MaxHealth - i * game.Players[0].Strength, game.Players[1].Health);
            for (int j = 0; j <= AttackTickInterval; j++)
            {
                game.Players[0].Attack(game.Players[1].PlayerPosition.X, game.Players[1].PlayerPosition.Y);
                game.Tick();
                Assert.StrictEqual(game.Players[1].MaxHealth - i * game.Players[0].Strength, game.Players[1].Health);
            }
        }
        Assert.False(game.Players[1].IsAlive);
        Assert.Null(game.Winner);

        // BedDestryed
        game.Players[0].Attack(game.Players[1].SpawnPoint.X, game.Players[1].SpawnPoint.Y);
        game.Tick();
        Assert.Null(game.Winner);
        game.Players[0].Move(game.Players[1].SpawnPoint.X, game.Players[1].SpawnPoint.Y);
        game.Tick();
        game.Players[0].Attack(game.Players[1].SpawnPoint.X, game.Players[1].SpawnPoint.Y);
        game.Tick();
        Assert.StrictEqual(IGame.Stage.Finished, game.CurrentStage);
        Assert.StrictEqual(game.Players[0], game.Winner);

    }
}