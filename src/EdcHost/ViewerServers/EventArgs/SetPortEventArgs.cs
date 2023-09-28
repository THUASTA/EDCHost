namespace EdcHost.ViewerServers.EventArgs;

public class SetPortEventArgs : System.EventArgs
{
    public int PlayerId { get; }
    public string PortName { get; }
    public int BaudRate { get; }

    public SetPortEventArgs(int playerId, string portName, int baudRate)
    {
        PlayerId = playerId;
        PortName = portName;
        BaudRate = baudRate;
    }
}
