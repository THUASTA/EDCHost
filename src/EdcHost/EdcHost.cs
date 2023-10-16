using System.IO.Ports;
using EdcHost.Games;
using EdcHost.SlaveServers;
using EdcHost.ViewerServers;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    /// <summary>
    /// Default serial ports.
    /// </summary>
    public readonly string[] DefaultSerialPorts = { "COM1", "COM2" };

    /// <summary>
    /// Default baud rates.
    /// </summary>
    public readonly int[] DefaultBaudRates = { 19200, 19200 };

    /// <summary>
    /// Default viewer server port.
    /// </summary>
    public const int DefaultViewerServerPort = 3001;

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

        string[] allPorts = SerialPort.GetPortNames();
        List<string> availablePorts = new();
        foreach (string port in allPorts)
        {
            try
            {
                // Try open the port 
                using (SerialPort serialPort = new SerialPort(port))
                {
                    serialPort.Open();
                }

                // Port is available if the port is opened successfully
                Serilog.Log.Information($"Port {port} is available.");
                availablePorts.Add(port);
            }
            catch (Exception ex)
            {
                // Port is not available if the port cannot be opened 
                Serilog.Log.Warning($"Port {port} is not available: {ex.Message}");
            }
        }

        if (availablePorts.Count < 2)
        {
            Serilog.Log.Fatal("Not enough available ports.");
        }

        string[] ports = new string[] { availablePorts[0], availablePorts[1] };

        /// <remarks>
        /// Choose ports and baudrates here
        /// </remarks>
        _slaveServer = new SlaveServer(ports, DefaultBaudRates);
        _viewerServer = new ViewerServer(DefaultViewerServerPort);

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
            Serilog.Log.Information("Starting slave server.");
            _slaveServer.Start();
            Serilog.Log.Information("Starting game.");
            _game.Start();
            Serilog.Log.Information("Starting viewer server.");
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
            Serilog.Log.Information("Stopping viewer server.");
            _viewerServer.Stop();
            Serilog.Log.Information("Stopping game.");
            _game.Stop();
            Serilog.Log.Information("Stopping slave server.");
            _slaveServer.Stop();
            Serilog.Log.Information("Host stopped.");
        }
        catch (Exception e)
        {
            Serilog.Log.Warning($"Host stopped with exception: {e}");
        }
    }
}
