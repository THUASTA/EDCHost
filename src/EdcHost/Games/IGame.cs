namespace EdcHost.Games;

/// <summary>
/// Game handles the game logic.
/// </summary>
public interface IGame
{
    /// <summary>
    /// Stage of game.
    /// </summary>
    public enum Stage
    {
        Ready,
        Running,
        Battling,
        Finished,
        Ended,
    }

    public event EventHandler<AfterGameStartEventArgs>? AfterGameStartEvent;
    public event EventHandler<AfterGameTickEventArgs>? AfterGameTickEvent;
    public event EventHandler<AfterJudgementEventArgs>? AfterJudgementEvent;

    /// <summary>
    /// Current stage of the game.
    /// </summary>
    public Stage CurrentStage { get; }

    /// <summary>
    /// Elapsed time of the game.
    /// </summary>
    public int ElapsedTicks { get; }

    /// <summary>
    /// Winner of the game.
    /// </summary>
    /// <remarks>
    /// Winner can be null in case there is no winner.
    /// </remarks>
    public IPlayer? Winner { get; }

    /// <summary>
    /// The players.
    /// </summary>
    public List<IPlayer> Players { get; }

    /// <summary>
    /// The game map.
    /// </summary>
    public IMap GameMap { get; }

    /// <summary>
    /// The mines.
    /// </summary>
    public List<IMine> Mines { get; }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public Task Start();

    /// <summary>
    /// Ends the game.
    /// </summary>
    public Task End();
}
