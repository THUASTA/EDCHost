namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryAttackEventArgs : System.EventArgs
{
    public string PortName { get; }
    public int TargetChunkId { get; }

    public PlayerTryAttackEventArgs(string portName, int targetChunkId)
    {
        PortName = portName;
        TargetChunkId = targetChunkId;
    }
}
