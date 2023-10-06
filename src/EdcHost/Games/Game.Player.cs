namespace EdcHost.Games;

public partial class Game : IGame
{
    /// <summary>
    /// Maximum count of same type of items a player can hold.
    /// </summary>
    public const int MaximumItemCount = 64;

    /// <summary>
    /// The damage which will kill a player instantly.
    /// </summary>
    private const int InstantDeathDamage = 255;

    /// <summary>
    /// All players.
    /// </summary>
    public List<IPlayer> Players { get; private set; }

    /// <summary>
    /// Whether all beds are destroyed or not.
    /// </summary>
    private bool _allBedsDestroyed;

    /// <summary>
    /// Last attack time of each player.
    /// </summary>
    private readonly List<DateTime> _playerLastAttackTime;

    /// <summary>
    /// Last time a player dies.
    /// </summary>
    private readonly List<DateTime?> _playerDeathTime;

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
            IPlayer.CommodityKindType.AgilityBoost => (int)Math.Pow(2, player.ActionPoints),
            IPlayer.CommodityKindType.HealthBoost => player.MaxHealth - 20,
            IPlayer.CommodityKindType.StrengthBoost => (int)Math.Pow(2, player.Strength),
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
        return TimeSpan.FromSeconds((double)10.0d / (double)player.ActionPoints);
    }

    /// <summary>
    /// Gets the opponent of a player.
    /// </summary>
    /// <param name="player">The player</param>
    /// <returns>Opponent of the player</returns>
    private IPlayer Opponent(IPlayer player)
    {
        //0^1=1, 1^1=0, 0^0=0
        return Players[player.PlayerId ^ 1];
    }

}
