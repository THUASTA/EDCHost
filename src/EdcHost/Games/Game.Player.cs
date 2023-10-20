namespace EdcHost.Games;

public partial class Game : IGame
{
    private const int TicksBeforeRespawn = 300;

    /// <summary>
    /// Number of players.
    /// </summary>
    public const int PlayerNum = 2;

    /// <summary>
    /// Maximum count of same type of items a player can hold.
    /// </summary>
    public const int MaximumItemCount = 64;

    /// <summary>
    /// The damage which will kill a player instantly.
    /// </summary>
    private const int InstantDeathDamage = 114514;

    /// <summary>
    /// All players.
    /// </summary>
    public List<IPlayer> Players { get; private set; }

    /// <summary>
    /// Whether all beds are destroyed or not.
    /// </summary>
    private bool _isAllBedsDestroyed;

    private readonly List<int?> _playerDeathTickList;
    private readonly List<int> _playerLastAttackTickList;

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

    private int AttackTickInterval(IPlayer player)
    {
        return 200 / player.ActionPoints;
    }

    private IPlayer Opponent(IPlayer player)
    {
        return Players[(player.PlayerId == 0) ? 1 : 0];
    }

}
