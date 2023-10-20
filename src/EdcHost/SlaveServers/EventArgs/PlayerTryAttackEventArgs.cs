namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryAttackEventArgs : System.EventArgs
{
    public string EventType { get; } = "PLAYER_TRY_ATTACK";
    public string PortName { get; }
    public int TargetChunk { get; }

    public PlayerTryAttackEventArgs(string portName, int targetChunk)
    {
        PortName = portName;
        TargetChunk = targetChunk;
    }
}
