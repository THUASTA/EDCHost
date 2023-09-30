using EdcHost.Games;
using Moq;
using Serilog.Core;
using Serilog.Events;
using Xunit;
using System.Reflection; 

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
        Assert.Null((DateTime)game.GetType().GetProperty("_startTime", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(game));  
        Assert.Null((DateTime)game.GetType().GetProperty("_lastTickTime", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(game)); 
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
        DateTime now = DateTime.Now;
        DateTime actualStartTime = (DateTime)game.GetType().GetProperty("_startTime", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(game);
        TimeSpan timeDifference = now - actualStartTime;
        Assert.True(timeDifference.TotalMilliseconds < 1);
        Assert.Equal(TimeSpan.FromSeconds(0), game.ElapsedTime); 
    }

    [Fact]
    public void Stop_NotStart_LogsWarning()
    {
        
        
        
    }

}
    public class GameTests
    {
        [Fact]
        public void Start_Game_Should_Set_Correct_Initial_Values()
        {
            // Arrange
            var game = new Game();

            // Act
            game.Start();

            // Assert
            Assert.Equal(GameStage.Battle, GetPrivateFieldValue<GameStage>(game, "_currentStage"));
            // Add more assertions to verify other initial values
        }

        [Fact]
        public void Stop_Game_Should_Reset_Game_State()
        {
            // Arrange
            var game = new Game();
            game.Start();

            // Act
            game.Stop();

            // Assert
            Assert.Equal(GameStage.NotStarted, GetPrivateFieldValue<GameStage>(game, "_currentStage"));
            // Add more assertions to verify other reset values
        }

        // Example test for private method
        [Fact]
        public void GenerateMines_Should_Create_Mines_According_To_Game_Rules()
        {
            // Arrange
            var game = new Game();
            var minesField = typeof(Game).GetField("_mines", BindingFlags.NonPublic | BindingFlags.Instance);
            var mines = minesField.GetValue(game) as List<Mine>;

            // Act
            var methodInfo = typeof(Game).GetMethod("GenerateMines", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo.Invoke(game, null);

            // Assert
            // Add assertions to verify that the mines are generated correctly
            Assert.NotNull(mines);
            Assert.NotEmpty(mines);
        }

        private T GetPrivateFieldValue<T>(object obj, string fieldName)
        {
            var fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)fieldInfo.GetValue(obj);
        }
    }
}