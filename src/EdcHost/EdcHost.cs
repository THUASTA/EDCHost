using EdcHost.Games;
using EdcHost.SlaveServers;
using EdcHost.ViewerServers;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    /// <summary>
    /// The game.
    /// </summary>
    private readonly IGame _game;

    /// <summary>
    /// The slave server.
    /// </summary>
    private readonly ISlaveServer _slaveServer;

    /// <summary>
    /// The viewer server.
    /// </summary>
    private readonly IViewerServer _viewerServer;

    public EdcHost()
    {
        _game = new Game();

        /// <remarks>
        /// Choose ports and baudrates here
        /// </remarks>
        _slaveServer = new SlaveServer(new string[] { "COM1", "COM2" }, new int[] { 19200, 19200 });
        _viewerServer = new ViewerServer(3001);

        _game.AfterGameStartEvent += HandleAfterGameStartEvent;
        _game.AfterGameTickEvent += HandleAfterGameTickEvent;
        _game.AfterJudgementEvent += HandleAfterJudgementEvent;

        _slaveServer.PlayerTryAttackEvent += HandlePlayerTryAttackEvent;
        _slaveServer.PlayerTryTradeEvent += HandlePlayerTryTradeEvent;
        _slaveServer.PlayerTryUseEvent += HandlePlayerTryUseEvent;

        _viewerServer.SetCameraEvent += HandleSetCameraEvent;
        _viewerServer.SetPortEvent += HandleSetPortEvent;

        Start();
    }

    public void Start()
    {
        try
        {
            _slaveServer.Start();
            _game.Start();
            _viewerServer.Start();
            Serilog.Log.Information("Host started successfully.");
        }
        catch (Exception e)
        {
            Serilog.Log.Fatal($"An error occurred when starting host: {e}");
        }
    }

    public void Stop()
    {
        try
        {
            _viewerServer.Stop();
            _game.Stop();
            _slaveServer.Stop();
            Serilog.Log.Information("Host stopped.");
        }
        catch (Exception e)
        {
            Serilog.Log.Warning($"Host stopped with exception: {e}");
        }
    }
}
