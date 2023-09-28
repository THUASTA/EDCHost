namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryUseEventArgs : System.EventArgs
{
    public string EventType { get; }
    public int PlayerId { get; }
    public int TargetChunk { get; }

    public PlayerTryUseEventArgs(int playerId, int targetChunk)
    {
        EventType = new string("PLAYER_TRY_USE");
        PlayerId = playerId;
        TargetChunk = targetChunk;
    }
}
