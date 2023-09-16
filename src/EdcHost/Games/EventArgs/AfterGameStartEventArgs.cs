namespace EdcHost.Games;

public class AfterGameStartEventArgs : EventArgs
{
    /// <summary>
    /// The game.
    /// </summary>
    public IGame Game { get; }

    /// <summary>
    /// Start time of the game.
    /// </summary>
    public DateTime? StartTime { get; }

    public AfterGameStartEventArgs(IGame game, DateTime? startTime)
    {
        Game = game;
        StartTime = startTime;
    }
}
