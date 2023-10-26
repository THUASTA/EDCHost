using Serilog;
using System.Collections.Concurrent;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    const int _mapWidth = 10;
    const int _mapHeight = 10;

    readonly Games.IGame _game;
    readonly Games.IGameRunner _gameRunner;
    readonly ILogger _logger = Log.ForContext("Component", "Program");
    readonly Dictionary<int, string> _playerIdToPortName = new();
    readonly SlaveServers.ISlaveServer _slaveServer;
    readonly ViewerServers.IViewerServer _viewerServer;
    /// <summary>
    /// store the player event for every tick in order to transfer to the viewerServer
    /// </summary>
    private ConcurrentQueue<EventArgs> _playerEventQueue = new();

    public EdcHost(Games.IGame game, Games.IGameRunner gameRunner, SlaveServers.ISlaveServer slaveServer, ViewerServers.IViewerServer viewerServer)
    {
        _game = game;
        _gameRunner = gameRunner;
        _slaveServer = slaveServer;
        _viewerServer = viewerServer;

        _game.AfterGameStartEvent += HandleAfterGameStartEvent;
        _game.AfterGameTickEvent += HandleAfterGameTickEvent;
        _game.AfterJudgementEvent += HandleAfterJudgementEvent;

        for (int i = 0; i < _game.Players.Count; i++)
        {
            _game.Players[i].OnAttack += HandlePlayerAttackEvent;
            _game.Players[i].OnPlace += HandlePlayerPlaceEvent;
            // TODO: Add OnDig and OnPickUp Event Handlers
        }

        _slaveServer.PlayerTryAttackEvent += HandlePlayerTryAttackEvent;
        _slaveServer.PlayerTryTradeEvent += HandlePlayerTryTradeEvent;
        _slaveServer.PlayerTryPlaceBlockEvent += HandlePlayerTryPlaceBlockEvent;

        _viewerServer.SetCameraEvent += HandleSetCameraEvent;
        _viewerServer.SetPortEvent += HandleSetPortEvent;
        _viewerServer.Controller.StartGameEvent += HandleStartGameEvent;
        _viewerServer.Controller.EndGameEvent += HandleEndGameEvent;
        _viewerServer.Controller.ResetGameEvent += HandleResetGameEvent;
    }

    public void Start()
    {
        _logger.Information("Starting...");

        try
        {
            _slaveServer.Start();
        }
        catch (Exception e)
        {
            _logger.Error($"failed to start slave server: {e}");
        }

        try
        {
            _viewerServer.Start();
        }
        catch (Exception e)
        {
            _logger.Error($"failed to start viewer server: {e}");
        }

        _logger.Information("Started.");

        // Main thread sleeps to guarantee that the program is running
        Thread.Sleep(Timeout.Infinite);

    }

    public void Stop()
    {
        _logger.Information("Stopping...");

        try
        {
            _slaveServer.Stop();
        }
        catch (Exception e)
        {
            _logger.Error($"failed to stop slave server: {e}");
        }

        try
        {
            _viewerServer.Stop();
        }
        catch (Exception e)
        {
            _logger.Error($"failed to stop viewer server: {e}");
        }

        _logger.Information("Stopped.");
    }
}
