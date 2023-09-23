using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class GameTests
{
    [Fact]
    public void Game_DoNothing_CorrectlyInitialized()
    {
        var game = new Game();
        Assert.Equal(IGame.Stage.Ready, game.CurrentStage);
        TimeSpan expElapsedTime = TimeSpan.FromSeconds(0);
        Assert.Equal(expElapsedTime, game.ElapsedTime);
        Assert.Null(game.Winner);
        Assert.Null(game._startTime);
        Assert.Null(game._lastTickTime);
        Assert.Null(game._lastOreGeneratedTime);
        //TODO:Test _map....

    }
    /*    public Game()
    {
        CurrentStage = IGame.Stage.Ready;
        ElapsedTime = TimeSpan.FromSeconds(0);
        Winner = null;

        _startTime = null;
        _lastTickTime = null;
        _lastOreGeneratedTime = null;

        _map = new Map();
        _players = new(2);
        _mines = new();
        GenerateMines();

        _tickTask = new(Tick);
        //TODO: Add players
        //TODO: Subscribe player events
    }*/

    [Fact]
    public void Start_CurrectlyIntialized()
    {
        var game = new Game();
        game.Start();
        //Todo:Exception
        Assert.Equal(game)
        /*     CurrentStage = IGame.Stage.Running;
        Winner = null;

        DateTime initTime = DateTime.Now;
        _startTime = initTime;
        _lastTickTime = initTime;
        _lastOreGeneratedTime = initTime;
        ElapsedTime = TimeSpan.FromSeconds(0);

        _tickTask.Start();

        //TODO: Invoke game events
    }*/
    }
}
