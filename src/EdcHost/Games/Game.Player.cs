namespace EdcHost.Games;

partial class Game : IGame
{
    const int TicksBeforeRespawn = 300;

    /// <summary>
    /// Maximum count of same type of items a player can hold.
    /// </summary>
    public const int MaximumItemCount = 64;

    /// <summary>
    /// The damage which will kill a player instantly.
    /// </summary>
    const int InstantDeathDamage = 114514;

    /// <summary>
    /// This means a player hasn't done something (for example, Attack) yet
    /// after game started.
    /// </summary>
    const int Never = -3939;

    /// <summary>
    /// Number of players.
    /// </summary>
    public int PlayerNum { get; }

    /// <summary>
    /// All players.
    /// </summary>
    public List<IPlayer> Players { get; private set; }

    /// <summary>
    /// Whether all beds are destroyed or not.
    /// </summary>
    bool _isAllBedsDestroyed;

    readonly List<int?> _playerDeathTickList;
    readonly List<int> _playerLastAttackTickList;

    int CommodityValue(
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

    int AttackTickInterval(IPlayer player)
    {
        return 200 / player.ActionPoints;
    }

    IPlayer Opponent(IPlayer player)
    {
        return Players[(player.PlayerId == 0) ? 1 : 0];
    }

}
