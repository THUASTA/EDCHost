using EdcHost.Games;
using EdcHost.SlaveServers;
using EdcHost.ViewerServers;
using CompetitionUpdate = EdcHost.ViewerServers.Messages.CompetitionUpdate;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandleAfterGameStartEvent(object? sender, AfterGameStartEventArgs e)
    {
        //Do nothing
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

            for (int i = 0; i < 2; i++)
            {
                string? portName = _playerIdToPortName.GetValueOrDefault(e.Game.Players[i].PlayerId);
                if (portName is null)
                {
                    continue;
                }

                _slaveServer.Publish(
                    portName: portName,
                    gameStage: (int)e.Game.CurrentStage,
                    elapsedTime: e.Game.ElapsedTicks,
                    heightOfChunks: heightOfChunks,
                    hasBed: e.Game.Players[i].HasBed,
                    hasBedOpponent: e.Game.Players.Any(player => player.HasBed && player.PlayerId != e.Game.Players[i].PlayerId),
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

            // Send packet to the viewer
            List<CompetitionUpdate.Event> currentEvents = new();
            while (!this._playerEventQueue.IsEmpty)
            {
                currentEvents.Add(new CompetitionUpdate.Event()
                {

                });
            }

            _viewerServer.Publish(new CompetitionUpdate()
            {
                // TODO: Add cameras
                cameras = new List<CompetitionUpdate.Camera>(),

                chunks = e.Game.GameMap.Chunks.Select(chunk => new CompetitionUpdate.Chunk()
                {
                    chunkId = chunk.Position != null ? chunk.Position.X + chunk.Position.Y * 8 : -1,
                    height = chunk.Height,
                    position = chunk.Position != null ? new CompetitionUpdate.Chunk.Position()
                    {
                        x = chunk.Position.X,
                        y = chunk.Position.Y
                    } : null
                }).ToList(),

                // TODO: Add events
                events = currentEvents,

                info = new CompetitionUpdate.Info()
                {
                    // TODO: Add 'switch'
                    stage = (CompetitionUpdate.Info.Stage)(e.Game.CurrentStage),
                    elapsedTicks = e.Game.ElapsedTicks
                },

                mines = e.Game.Mines.Select(mine => new CompetitionUpdate.Mine()
                {
                    // TODO: Add 'MineId'
                    mineId = (int)(mine.OreKind), // error

                    accumulatedOreCount = mine.AccumulatedOreCount,
                    oreType = (CompetitionUpdate.Mine.OreType)mine.OreKind,
                    position = new CompetitionUpdate.Mine.Position()
                    {
                        x = mine.Position.X,
                        y = mine.Position.Y
                    }
                }).ToList(),

                players = e.Game.Players.Select(player => new CompetitionUpdate.Player()
                {
                    playerId = (player.PlayerId),
                    attributes = new()
                    {
                        agility = player.ActionPoints,
                        strength = player.Strength,
                        maxHealth = player.MaxHealth
                    },
                    health = player.Health,
                    homePosition = new CompetitionUpdate.Player.HomePosition()
                    {
                        x = player.SpawnPoint.X,
                        y = player.SpawnPoint.Y,
                    },
                    inventory = new CompetitionUpdate.Player.Inventory()
                    {
                        emerald = player.EmeraldCount,
                        wool = player.WoolCount
                    },
                    position = new CompetitionUpdate.Player.Position()
                    {
                        x = player.PlayerPosition.X,
                        y = player.PlayerPosition.Y
                    }
                }).ToList()
            });
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

        _gameRunner.End();
    }

    void HandlePlayerAttackEvent(object? sender, Games.PlayerAttackEventArgs e)
    {
        // Store the event info to the queue
        this._playerEventQueue.Enqueue(e);
    }
    void HandlePlayerPlaceEvent(object? sender, Games.PlayerPlaceEventArgs e)
    {
        // Store the event info to the queue
        this._playerEventQueue.Enqueue(e);
    }
}
