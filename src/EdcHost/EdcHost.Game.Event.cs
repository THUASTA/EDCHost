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
        //TODO: Update packet to send after new game tick
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
    }
}
