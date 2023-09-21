namespace EdcHost.Games;

public class Player : IPlayer
{
    public int PlayerId { get; private set; }
    public int EmeraldCount { get; private set; }
    public bool HasBed { get; private set; }
    public bool IsAlive { get; private set; }
    public IPosition<float> SpawnPoint { get; private set; }
    public IPosition<float> PlayerPosition { get; private set; }
    public int WoolCount { get; private set; }

    public int Health { get; private set; } /// Health property
    public int MaxHealth { get; private set; } /// Max health property
    public int Strength { get; private set; } /// Strength property
    public int ActionPoints { get; private set; } /// Action points property

    public event EventHandler<PlayerMoveEventArgs> OnMove = delegate { };
    public event EventHandler<PlayerAttackEventArgs> OnAttack = delegate { };
    public event EventHandler<PlayerPlaceEventArgs> OnPlace = delegate { };
    public event EventHandler<PlayerDieEventArgs> OnDie = delegate { };

    public void Move(float newX, float newY)
    {
        /// Trigger the OnMove event to notify other parts that the player has moved
        OnMove?.Invoke(this, new PlayerMoveEventArgs(this, PlayerPosition, new Position<float>(newX, newY)));
        /// Update the player's position information
        PlayerPosition.X = newX;
        PlayerPosition.Y = newY;
    }

    public void Attack(float newX, float newY)
    {
        /// Trigger the OnAttack event to notify the attacked block
        OnAttack?.Invoke(this, new PlayerAttackEventArgs(this, Strength, new Position<float>(newX, newY)));
    }
    public void Place(float newX, float newY)
    {
        /// Check if the player has wool in their inventory, and if so, process wool data.
        ///  Trigger the OnPlace event to notify the placed block
        if (WoolCount > 0)
        {
            OnPlace?.Invoke(this, new PlayerPlaceEventArgs(this, new Position<float>(newX, newY)));
        }
    }
    public void Hurt(int EnemyStrength)
    {
        /// Implement the logic for being hurt
        if (Health > EnemyStrength)
        {
            Health -= EnemyStrength;
        }
        else
        {
            Health = 0;
            IsAlive = false;
            OnDie?.Invoke(this, new PlayerDieEventArgs(this));
        }
    }
    public Player()
    {
        PlayerId = 1;
        EmeraldCount = 0;
        IsAlive = true;
        HasBed = true;

        /// Initialize SpawnPoint
        SpawnPoint = new Position<float>(0, 0); /// Initial coordinates can be set as needed
        PlayerPosition = new Position<float>(0, 0); /// Initial coordinates can be set as needed
        WoolCount = 0;

        /// Initialize player attributes
        Health = 20; /// Initial health
        MaxHealth = 20; /// Initial max health
        Strength = 1; /// Initial strength
        ActionPoints = 1; /// Initial action points
    }
    public Player(int id, float initialX, float initialY, float initialX2, float initialY2)
    {
        PlayerId = id;
        EmeraldCount = 0;
        IsAlive = true;
        HasBed = true;
        SpawnPoint = new Position<float>(initialX, initialY);
        PlayerPosition = new Position<float>(initialX2, initialY2);
        WoolCount = 0;

        Health = 20; /// Initial health
        MaxHealth = 20; /// Initial max health
        Strength = 1; /// Initial strength
        ActionPoints = 1; /// Initial action points
        ActionPoints = 1;
    }
    public void PerformActionPosition(IPlayer.ActionKindType actionKind, float X, float Y)
    {
        switch (actionKind)
        {
            case IPlayer.ActionKindType.Attack:
                /// Implement the logic for attacking
                Attack(X, Y);
                break;
            case IPlayer.ActionKindType.PlaceBlock:
                /// Implement the logic for placing a block
                Place(X, Y);
                break;
            default:
                /// Handle unknown action types
                break;
        }
    }
    public bool Trade(IPlayer.CommodityKindType commodityKind)
    {
        switch (commodityKind)
        {
            case IPlayer.CommodityKindType.AgilityBoost:
                int price = (int)Math.Pow(2, ActionPoints);
                if (EmeraldCount >= Math.Pow(2, ActionPoints))
                {
                    EmeraldCount -= (int)Math.Pow(2, ActionPoints);
                    ActionPoints += 1;
                    return true;
                }
                break;
            case IPlayer.CommodityKindType.HealthBoost:
                if (EmeraldCount >= (20 - MaxHealth))
                {
                    EmeraldCount -= 20 - MaxHealth;
                    MaxHealth += 1;
                    return true;
                }
                /// Implement the logic for health boost
                /// You can perform the health boost operation here and update the player's status
                break;
            case IPlayer.CommodityKindType.HealthPotion:
                if (EmeraldCount >= 4 && Health < MaxHealth)
                {
                    EmeraldCount -= 4;
                    Health += 1;
                    return true;
                }
                /// Implement the logic for health potion
                /// You can perform the health potion use operation here and update the player's status
                break;
            case IPlayer.CommodityKindType.StrengthBoost:
                if (EmeraldCount >= Math.Pow(2, ActionPoints))
                {
                    EmeraldCount -= (int)Math.Pow(2, ActionPoints);
                    Strength += 1;
                    return true;
                }
                /// Implement the logic for strength boost
                /// You can perform the strength boost operation here and update the player's status
                break;
            case IPlayer.CommodityKindType.Wool:
                if (EmeraldCount >= 1)
                {
                    EmeraldCount -= 1;
                    WoolCount += 1;
                    return true;
                }
                break;
            default:
                /// Handle unknown commodity types
                break;
        }
        return false;
    }
}
