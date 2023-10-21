namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryTradeEventArgs : System.EventArgs
{
    public string EventType { get; }
    public string PortName { get; }
    public int Item { get; }

    public PlayerTryTradeEventArgs(string portName, int item)
    {
        EventType = new string("PLAYER_TRY_TRADE");
        PortName = portName;
        Item = item;
    }
}
