using EdcHost.Games;
using EdcHost.SlaveServers;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandleAfterGameStartEvent(object? sender, AfterGameStartEventArgs e)
    {

    }

    void HandleAfterGameTickEvent(object? sender, AfterGameTickEventArgs e)
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
                string? portName = _playerIdToPortName.GetValueOrDefault(e.Game.Players[i].PlayerId);
                if (portName is null)
                {
                    continue;
                }

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

                _slaveServer.Publish(
                    portName: portName,
                    gameStage: (int)e.Game.CurrentStage,
                    elapsedTime: e.Game.ElapsedTicks,
                    heightOfChunks: heightOfChunks,
                    hasBed: e.Game.Players[i].HasBed,
                    positionX: e.Game.Players[i].PlayerPosition.X,
                    positionY: e.Game.Players[i].PlayerPosition.Y,
                    positionOpponentX: e.Game.Players[(i == 0) ? 1 : 0].PlayerPosition.X,
                    positionOpponentY: e.Game.Players[(i == 0) ? 1 : 0].PlayerPosition.Y,
                    agility: e.Game.Players[i].ActionPoints,
                    health: e.Game.Players[i].Health,
                    maxHealth: e.Game.Players[i].MaxHealth,
                    strength: e.Game.Players[i].Strength,
                    emeraldCount: e.Game.Players[i].EmeraldCount,
                    woolCount: e.Game.Players[i].WoolCount
                );
            }
        }
        catch (Exception exception)
        {
            _logger.Warning($"An exception is caught when updating packet: {exception}");
        }
    }

    void HandleAfterJudgementEvent(object? sender, AfterJudgementEventArgs e)
    {
        if (e.Winner is null)
        {
            _logger.Information("No winner.");
        }
        else
        {
            _logger.Information($"Winner is {e.Winner?.PlayerId}");
        }

        Stop();
    }
}
