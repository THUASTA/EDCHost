namespace EdcHost.Games;

/// <summary>
/// Player represents a player in the game.
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// The kind of a commodity.
    /// </summary>
    public enum CommodityKindType
    {
        AgilityBoost,
        HealthBoost,
        HealthPotion,
        StrengthBoost,
        Wool,
    }

    /// <summary>
    /// The kind of an action.
    /// </summary>
    public enum ActionKindType
    {
        Attack,
        PlaceBlock

    }

    /// <summary>
    /// Triggered when the player moves.
    /// </summary>
    event EventHandler<PlayerMoveEventArgs> OnMove;
    event EventHandler<PlayerAttackEventArgs> OnAttack;
    event EventHandler<PlayerPlaceEventArgs> OnPlace;
    event EventHandler<PlayerDieEventArgs> OnDie;

    public void EmeraldAdd(int count);
    public void Move(float newX, float newY);
    public void Attack(float newX, float newY);
    public void Place(float newX, float newY);
    public void Hurt(int EnemyStrength);
    public void Spawn(int MaxHealth);
    public void DestroyBed();
    public void DecreaseWoolCount();
    /// <summary>
    /// The Id of  the player 
    /// </summary>
    public int PlayerId { get; }
    /// <summary>
    /// The count of emeralds the player has.
    /// </summary>
    public int EmeraldCount { get; }
    /// <summary>
    /// Whether the player is alive.
    /// </summary>
    public bool IsAlive { get; }
    /// <summary>
    /// Whether the player has a bed.
    /// </summary>
    public bool HasBed { get; }
    /// <summary>
    /// Whether the opponent player has a bed.
    /// </summary>
    public bool HasBedOpponent{get;}
    /// <summary>
    /// The spawn point of the player.
    /// </summary>
    public IPosition<float> SpawnPoint { get; }
    /// <summary>
    /// The position of the player.
    /// </summary>
    public IPosition<float> PlayerPosition { get; }
    /// <summary>
    /// The count of wool blocks the player has.
    /// </summary>
    public int WoolCount { get; }
    public int Health { get; }
    public int MaxHealth { get; }
    public int Strength { get; }
    public int ActionPoints { get; }
    /// <summary>
    /// Performs an action.
    /// </summary>
    /// <param name="actionKind">The action kind.</param>
    public void PerformActionPosition(IPlayer.ActionKindType actionKind, float X, float Y);
    /// <summary>
    /// Trades a commodity.
    /// </summary>
    /// <param name="commodityKind">The commodity kind.</param>
    public bool Trade(CommodityKindType commodityKind);
}
