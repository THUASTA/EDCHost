using EdcHost.Games;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    private void HandleAfterGameStartEvent(object? sender, AfterGameStartEventArgs e)
    {
        Serilog.Log.Information("Game started.");
    }

    private void HandleAfterGameTickEvent(object? sender, AfterGameTickEventArgs e)
    {
        try
        {
            //TODO: Call UpdatePacket in _slaveServer
            Serilog.Log.Information($"{e.Game.ElapsedTime} {e.Game.CurrentStage}\n");
            for (int i = 0; i < 2; i++)
            {
                Serilog.Log.Information($"Player {e.Game.Players[i].PlayerId} (Has bed: {e.Game.Players[i].HasBed}):");
                Serilog.Log.Information($"Holding {e.Game.Players[i].WoolCount} wools {e.Game.Players[i].EmeraldCount} emeralds");
                Serilog.Log.Information($"Position: ({e.Game.Players[i].PlayerPosition.X}, {e.Game.Players[i].PlayerPosition.Y})");
                Serilog.Log.Information($"Health: {e.Game.Players[i].Health}/{e.Game.Players[i].MaxHealth}");
                Serilog.Log.Information($"Strength: {e.Game.Players[i].Strength}");
                Serilog.Log.Information($"Agility: {e.Game.Players[i].ActionPoints}\n");
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"An exception is caught when updating game: {exception}");
        }
    }

    private void HandleAfterJudgementEvent(object? sender, AfterJudgementEventArgs e)
    {
        if (e.Winner is null)
        {
            Serilog.Log.Information("No winner.");
        }
        else
        {
            Serilog.Log.Information($"Winner is {e.Winner?.PlayerId}");
        }

        Stop();
    }
}
