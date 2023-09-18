namespace EdcHost.Games;

public class PlayerPlaceEventArgs : EventArgs
{
    /// <summary>
    /// The player that moved.
    /// </summary>
    public IPlayer Player { get; }

    /// <summary>
    /// The position the player place a block.
    /// </summary>
    public IPosition<float> Position { get; }

    public PlayerPlaceEventArgs(IPlayer player, IPosition<float> position)
    {
        Player = player;
        Position = position;
    }
}
