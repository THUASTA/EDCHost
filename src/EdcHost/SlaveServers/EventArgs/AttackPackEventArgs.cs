namespace EdcHost.SlaveServers.EventArgs;

public class AttackPackEventArgs : System.EventArgs
{
    public int PlayerId;
    public int Chunk { get; }

    public AttackPackEventArgs(int playerId, int chunk)
    {
        PlayerId = playerId;
        Chunk = chunk;
    }
}
