namespace EdcHost.SlaveServers.EventArgs;

public class PlacePackEventArgs : System.EventArgs
{
    public int PlayerId;
    public int Chunk { get; }

    public PlacePackEventArgs(int playerId, int chunk)
    {
        PlayerId = playerId;
        Chunk = chunk;
    }
}
