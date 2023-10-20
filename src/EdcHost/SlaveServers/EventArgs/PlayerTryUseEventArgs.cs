namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryUseEventArgs : System.EventArgs
{
    public string EventType { get; }
    public string PortName { get; }
    public int TargetChunk { get; }

    public PlayerTryUseEventArgs(string portName, int targetChunk)
    {
        EventType = new string("PLAYER_TRY_USE");
        PortName = portName;
        TargetChunk = targetChunk;
    }
}
