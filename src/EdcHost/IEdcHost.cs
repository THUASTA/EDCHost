namespace EdcHost;

/// <summary>
/// EdcHost does all the work of the program.
/// </summary>
public interface IEdcHost
{
    class EdcHostOptions
    {
        public List<Tuple<int, int>> GameDiamondMines { get; }
        public List<Tuple<int, int>> GameGoldMines { get; }
        public List<Tuple<int, int>> GameIronMines { get; }
        public int ServerPort { get; }

        public EdcHostOptions(int serverPort, List<Tuple<int, int>>? gameDiamondMines = null, List<Tuple<int, int>>? gameGoldMines = null, List<Tuple<int, int>>? gameIronMines = null)
        {
            GameDiamondMines = gameDiamondMines ?? new();
            GameGoldMines = gameGoldMines ?? new();
            GameIronMines = gameIronMines ?? new();
            ServerPort = serverPort;
        }
    }

    static IEdcHost Create(EdcHostOptions options)
    {
        var game = Games.IGame.Create(
            diamondMines: options.GameDiamondMines,
            goldMines: options.GameGoldMines,
            ironMines: options.GameIronMines
        );
        var gameRunner = Games.IGameRunner.Create(game);
        SlaveServers.ISlaveServer slaveServer = SlaveServers.ISlaveServer.Create();
        ViewerServers.IViewerServer viewerServer = ViewerServers.IViewerServer.Create(options.ServerPort);

        return new EdcHost(
            game: game,
            gameRunner: gameRunner,
            slaveServer: slaveServer,
            viewerServer: viewerServer
        );
    }

    /// <summary>
    /// Starts the host.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the host.
    /// </summary>
    void Stop();
}
