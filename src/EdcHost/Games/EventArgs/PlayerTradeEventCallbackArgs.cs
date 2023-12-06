namespace EdcHost.Games;

public class PlayerTradeEventCallbackArgs : EventArgs
{
    public IPlayer Player { get; }

    public IPlayer.CommodityKindType CommodityKind { get; }

    /// <summary>
    /// Whether the trade succeeded or not.
    /// </summary>
    public bool Succeed { get; }

    public PlayerTradeEventCallbackArgs(IPlayer player, IPlayer.CommodityKindType commodityKind, bool succeed)
    {
        Player = player;
        CommodityKind = commodityKind;
        Succeed = succeed;
    }
}
