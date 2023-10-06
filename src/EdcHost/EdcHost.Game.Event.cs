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
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning(@$"An exception is caught when updating game: {exception}");
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
