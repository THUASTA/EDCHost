namespace EdcHost.SlaveServers.EventArgs;

public class PlayerTryTradeEventArgs : System.EventArgs
{
    public string EventType { get; }
    public int PlayerId { get; }
    public int Item { get; }

    public PlayerTryTradeEventArgs(int playerId, int item)
    {
        EventType = new string("PLAYER_TRY_TRADE");
        PlayerId = playerId;
        Item = item;
    }
}
