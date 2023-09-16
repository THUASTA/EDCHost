namespace EdcHost.Games;

public partial class Game : IGame
{
    /// <summary>
    /// Maximum count of same type of items a player can hold.
    /// </summary>
    private const int MaximumItemCount = 64;

    /// <summary>
    /// The damage which will kill a player instantly.
    /// </summary>
    private const int InstantDeathDamage = 255;

    /// <summary>
    /// All players.
    /// </summary>
    private readonly List<IPlayer> _players;

    /// <summary>
    /// PlayerMovementAction of last tick.
    /// </summary>
    private readonly Queue<PlayerMoveAction> _lastMovements;

    /// <summary>
    /// PlayerAttackAction of last tick.
    /// </summary>
    private readonly Queue<PlayerAttackAction> _lastAttacks;

    /// <summary>
    /// PlayerPlaceBlockAction of last tick.
    /// </summary>
    private readonly Queue<PlayerPlaceBlockAction> _lastPlaceActions;

    /// <summary>
    /// PlayerTradeAction of last tick.
    /// </summary>
    private readonly Queue<PlayerTradeAction> _lastTradeActions;

    /// <summary>
    /// Cauculate commodity value.
    /// </summary>
    /// <param name="player">The player</param>
    /// <param name="commodityKind">The commodity kind</param>
    /// <returns>The value</returns>
    /// <exception cref="ArgumentOutOfRangeException">No such commodity</exception>
    private int CommodityValue(
        IPlayer player, IPlayer.CommodityKindType commodityKind) => commodityKind switch
        {
            //TODO: Calculate value of Boosts accoding to player's property
            IPlayer.CommodityKindType.AgilityBoost => 0,
            IPlayer.CommodityKindType.HealthBoost => 0,
            IPlayer.CommodityKindType.StrengthBoost => 0,

            IPlayer.CommodityKindType.Wool => 1,
            IPlayer.CommodityKindType.HealthPotion => 4,
            _ => throw new ArgumentOutOfRangeException(
                nameof(commodityKind), $"No commodity {commodityKind}")
        };

    /// <summary>
    /// Time interval between two attack actions of a player.
    /// </summary>
    /// <param name="player">The player</param>
    /// <returns>Time interval</returns>
    private TimeSpan AttackTimeInterval(IPlayer player)
    {
        //TODO: Cauculate time interval according to agility
        return TimeSpan.FromSeconds(0);
    }

}
