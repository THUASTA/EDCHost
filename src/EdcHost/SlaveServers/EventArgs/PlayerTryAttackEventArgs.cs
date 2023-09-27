namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryAttackEventArgs : System.EventArgs
{
    public string EventType { get; }
    public int PlayerId { get; }
    public int TargetChunk { get; }

    public PlayerTryAttackEventArgs(int playerId, int targetChunk)
    {
        EventType = new string("PLAYER_TRY_ATTACK");
        PlayerId = playerId;
        TargetChunk = targetChunk;
    }
}
