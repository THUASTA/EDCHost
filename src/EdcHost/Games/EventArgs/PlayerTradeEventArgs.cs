namespace EdcHost.Games;

public class PlayerTradeEventArgs : EventArgs
{
    /// <summary>
    /// The player.
    /// </summary>
    public IPlayer Player { get; }

    /// <summary>
    /// The commodity kind.
    /// </summary>
    public IPlayer.CommodityKindType CommodityKind { get; }

    public PlayerTradeEventArgs(IPlayer player, IPlayer.CommodityKindType commodityKind)
    {
        Player = player;
        CommodityKind = commodityKind;
    }
}
