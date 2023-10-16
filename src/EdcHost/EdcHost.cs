using Serilog;

using EdcHost;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    private readonly Games.IGame _game;
    private readonly SlaveServers.ISlaveServer _slaveServer;
    private readonly ViewerServers.IViewerServer _viewerServer;

    public static IEdcHost Create(EdcHostOptions options)
    {
        Games.Game game = new();
        SlaveServers.SlaveServer slaveServer = new(new string[] { }, new int[] { });
        ViewerServers.ViewerServer viewerServer = new(options.ServerPort);

        return new EdcHost(
            game: game,
            slaveServer: slaveServer,
            viewerServer: viewerServer
        );
    }

    public EdcHost(Games.IGame game, SlaveServers.ISlaveServer slaveServer, ViewerServers.IViewerServer viewerServer)
    {
        _game = game;
        _slaveServer = slaveServer;
        _viewerServer = viewerServer;

        _game.AfterGameStartEvent += HandleAfterGameStartEvent;
        _game.AfterGameTickEvent += HandleAfterGameTickEvent;
        _game.AfterJudgementEvent += HandleAfterJudgementEvent;

        _slaveServer.PlayerTryAttackEvent += HandlePlayerTryAttackEvent;
        _slaveServer.PlayerTryTradeEvent += HandlePlayerTryTradeEvent;
        _slaveServer.PlayerTryUseEvent += HandlePlayerTryUseEvent;

        _viewerServer.SetCameraEvent += HandleSetCameraEvent;
        _viewerServer.SetPortEvent += HandleSetPortEvent;
    }

    public void Start()
    {
        Log.Information("Starting...");

        try
        {
            _slaveServer.Start();
        }
        catch (Exception e)
        {
            Log.Error($"failed to start slave server: {e}");
        }

        try
        {
            _viewerServer.Start();
        }
        catch (Exception e)
        {
            Log.Error($"failed to start viewer server: {e}");
        }

        Log.Information("Started.");
    }

    public void Stop()
    {
        Log.Information("Stopping...");

        try
        {
            _slaveServer.Stop();
        }
        catch (Exception e)
        {
            Log.Error($"failed to stop slave server: {e}");
        }

        try
        {
            _viewerServer.Stop();
        }
        catch (Exception e)
        {
            Log.Error($"failed to stop viewer server: {e}");
        }

        Log.Information("Stopped.");
    }
}
