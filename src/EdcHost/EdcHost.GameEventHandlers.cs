using EdcHost.Games;
using EdcHost.SlaveServers;

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
            List<int> heightOfChunks = new();
            foreach (IChunk chunk in e.Game.GameMap.Chunks)
            {
                heightOfChunks.Add(chunk.Height);
            }
            IPacket? packet = null;

            for (int i = 0; i < 2; i++)
            {
                packet = new PacketFromHost(
                    (int)e.Game.CurrentStage,
                    e.Game.ElapsedTicks * Game.TicksPerSecondExpected,
                    heightOfChunks,
                    e.Game.Players[i].HasBed,
                    e.Game.Players[i].PlayerPosition.X,
                    e.Game.Players[i].PlayerPosition.Y,
                    e.Game.Players[(i == 0) ? 1 : 0].PlayerPosition.X,
                    e.Game.Players[(i == 0) ? 1 : 0].PlayerPosition.Y,
                    e.Game.Players[i].ActionPoints,
                    e.Game.Players[i].Health,
                    e.Game.Players[i].MaxHealth,
                    e.Game.Players[i].Strength,
                    e.Game.Players[i].EmeraldCount,
                    e.Game.Players[i].WoolCount
                );
                _slaveServer.UpdatePacket(_game.Players[i].PlayerId, packet);
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"An exception is caught when updating packet: {exception}");
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
