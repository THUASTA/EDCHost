namespace EdcHost.SlaveServers.EventArgs;

public class TradePackEventArgs : System.EventArgs
{
    public int PlayerId;
    public int Item { get; }

    public TradePackEventArgs(int playerId, int item)
    {
        PlayerId = playerId;
        Item = item;
    }
}
