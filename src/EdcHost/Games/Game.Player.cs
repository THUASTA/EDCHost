namespace EdcHost.Games;

partial class Game : IGame
{
    const int TicksBeforeRespawn = 300;

    /// <summary>
    /// Maximum count of same type of items a player can hold.
    /// </summary>
    public const int MaximumItemCount = 128;

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

    /// <summary>
    /// Try to perform trade action.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <param name="commodityKind">The commodity kind.</param>
    public void TryTrade(IPlayer player, IPlayer.CommodityKindType commodityKind)
    {
        if (CurrentStage != IGame.Stage.Running)
        {
            _logger.Error($"Failed to trade: Trade is allowed at stage Running");
            return;
        }
        if (Players[player.PlayerId].IsAlive == false)
        {
            _logger.Error($"Failed to trade: Player {player.PlayerId} is dead.");
            return;
        }
        if (IsSamePosition(
            ToIntPosition(Players[player.PlayerId].PlayerPosition), Players[player.PlayerId].SpawnPoint
            ) == false)
        {
            _logger.Error($"Failed to trade: Player {player.PlayerId} is not at spawn point");
            return;
        }

        try
        {
            bool result = Players[player.PlayerId].Trade(commodityKind);
            if (result == false)
            {
                _logger.Error($"Failed to trade: Player {player.PlayerId} cannot buy {commodityKind}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to trade: {ex}");
        }
    }

    int AttackTickInterval(IPlayer player)
    {
        return (int)(Math.Max(8.5 - 0.25 * player.ActionPoints, 0.5) * TicksPerSecondExpected);
    }

}
