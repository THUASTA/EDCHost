namespace EdcHost;

public class EdcHostOptions
{
    private const int DefaultServerPort = 8080;

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
