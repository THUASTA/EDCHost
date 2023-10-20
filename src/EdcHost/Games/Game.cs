using System.Diagnostics;
using Serilog;

namespace EdcHost.Games;

/// <summary>
/// Game handles the game logic.
/// </summary>
partial class Game : IGame
{
    const int TickBattlingModeStart = 12000;
    public const int TicksPerSecondExpected = 20;

    /// <summary>
    /// Current stage of the game.
    /// </summary>
    public IGame.Stage CurrentStage { get; private set; } = IGame.Stage.Ready;

    /// <summary>
    /// Elapsed ticks of the game.
    /// </summary>
    public int ElapsedTicks { get; private set; } = 0;

    /// <summary>
    /// Winner of the game.
    /// </summary>
    /// <remarks>
    /// Winner can be null in case there is no winner.
    /// </remarks>
    public IPlayer? Winner { get; private set; } = null;

    /// <summary>
    /// The game map.
    /// </summary>
    public IMap GameMap { get; private set; }

    /// <summary>
    /// All mines.
    /// </summary>
    public List<IMine> Mines { get; private set; }

    readonly ILogger _logger = Log.Logger.ForContext("Component", "Games");

    public Game(List<Tuple<int, int>>? diamondMines,
        List<Tuple<int, int>>? goldMines, List<Tuple<int, int>>? ironMines)
    {
        var spawnPoints = new IPosition<int>[] { new Position<int>(0, 0), new Position<int>(7, 7) };
        GameMap = new Map(spawnPoints);

        Players = new();

        _playerLastAttackTickList = new();
        for (int i = 0; i < PlayerNum; i++)
        {
            _playerLastAttackTickList.Add(ElapsedTicks);
        }

        _playerDeathTickList = new();
        for (int i = 0; i < PlayerNum; i++)
        {
            _playerDeathTickList.Add(null);
        }

        Mines = new();
        GenerateMines(
            diamondMines: diamondMines,
            goldMines: goldMines,
            ironMines: ironMines
        );

        _isAllBedsDestroyed = false;

        Players.Clear();

        //TODO: Set player's initial position and spawnpoint

        Players.Add(new Player(0, 0.4f, 0.4f, 0.4f, 0.4f));
        Players.Add(new Player(1, 7.4f, 7.4f, 7.4f, 7.4f));

        for (int i = 0; i < PlayerNum; i++)
        {
            Players[i].OnMove += HandlePlayerMoveEvent;
            Players[i].OnAttack += HandlePlayerAttackEvent;
            Players[i].OnPlace += HandlePlayerPlaceEvent;
            Players[i].OnDie += HandlePlayerDieEvent;
        }

        for (int i = 0; i < PlayerNum; i++)
        {
            _playerLastAttackTickList[i] = ElapsedTicks;
        }

        for (int i = 0; i < PlayerNum; i++)
        {
            _playerDeathTickList[i] = null;
        }

        //TODO: Start game after all players are ready

        foreach (IMine mine in Mines)
        {
            mine.GenerateOre(ElapsedTicks);
        }

        Winner = null;

        _isAllBedsDestroyed = false;
    }

    public void Start()
    {
        _logger.Information("Starting...");

        if (CurrentStage != IGame.Stage.Ready)
        {
            throw new InvalidOperationException("the game has already started");
        }

        CurrentStage = IGame.Stage.Running;
        AfterGameStartEvent?.Invoke(this, new AfterGameStartEventArgs(this));

        _logger.Information("Started.");
    }

    public void End()
    {
        _logger.Information("Ending...");

        if (CurrentStage != IGame.Stage.Running && CurrentStage != IGame.Stage.Battling)
        {
            throw new InvalidOperationException("the game is not running");
        }

        CurrentStage = IGame.Stage.Ended;

        _logger.Information("Ended.");
    }

    public void Tick()
    {
        ++ElapsedTicks;

        if (IsFinished())
        {
            Judge();
            CurrentStage = IGame.Stage.Finished;
            return;
        }

        if (ElapsedTicks > TickBattlingModeStart && CurrentStage == IGame.Stage.Running)
        {
            CurrentStage = IGame.Stage.Battling;
        }

        if (CurrentStage == IGame.Stage.Battling)
        {
            if (_isAllBedsDestroyed == false)
            {
                for (int i = 0; i < PlayerNum; i++)
                {
                    Players[i].DestroyBed();
                }
                _isAllBedsDestroyed = true;
            }

            if ((ElapsedTicks - TickBattlingModeStart) % TicksPerSecondExpected == 0)
            {
                for (int i = 0; i < PlayerNum; i++)
                {
                    Players[i].Hurt(1);
                }
            }
        }

        UpdatePlayerInfo();
        UpdateMines();

        AfterGameTickEvent?.Invoke(
            this, new AfterGameTickEventArgs(this, ElapsedTicks));
    }

    void UpdatePlayerInfo()
    {
        for (int i = 0; i < PlayerNum; i++)
        {
            if (Players[i].HasBed == true
                && GameMap.GetChunkAt(ToIntPosition(Players[i].SpawnPoint)).IsVoid == true)
            {
                Players[i].DestroyBed();
            }

            if (Players[i].IsAlive == false && Players[i].HasBed == true
                && ElapsedTicks - _playerDeathTickList[i] > TicksBeforeRespawn
                && IsSamePosition(
                    ToIntPosition(Players[i].PlayerPosition), ToIntPosition(Players[i].SpawnPoint)
                    ) == true)
            {
                Players[i].Spawn(Players[i].MaxHealth);
                _playerDeathTickList[i] = null;

            }

            if (Players[i].IsAlive == true && IsValidPosition(
                ToIntPosition(Players[i].PlayerPosition)) == false)
            {
                Players[i].Hurt(InstantDeathDamage);
            }
            else if (Players[i].IsAlive == true && GameMap.GetChunkAt(
                ToIntPosition(Players[i].PlayerPosition)).IsVoid == true)
            {
                Players[i].Hurt(InstantDeathDamage);
            }
        }
    }

    /// <summary>
    /// Update mines.
    /// </summary>
    void UpdateMines()
    {
        foreach (IMine mine in Mines)
        {
            if (ElapsedTicks - mine.LastOreGeneratedTick >= mine.AccumulateOreInterval)
            {
                mine.GenerateOre(ElapsedTicks);
            }
            for (int i = 0; i < PlayerNum; i++)
            {
                if (Players[i].IsAlive == true
                    && IsSamePosition(
                        ToIntPosition(Players[i].PlayerPosition), ToIntPosition(mine.Position)
                        ) == true)
                {
                    //Remaining capacity of a player
                    int capacity = MaximumItemCount - Players[i].EmeraldCount;

                    //Value of an ore
                    int value = mine.OreKind switch
                    {
                        IMine.OreKindType.IronIngot => 1,
                        IMine.OreKindType.GoldIngot => 4,
                        IMine.OreKindType.Diamond => 16,
                        _ => throw new ArgumentOutOfRangeException(nameof(mine.OreKind), "No such ore kind.")
                    };

                    //Collected ore count
                    int collectedOre = Math.Min(capacity / value, mine.AccumulatedOreCount);

                    Players[i].EmeraldAdd(collectedOre * value);
                    mine.PickUpOre(collectedOre);
                }
            }
        }
    }

    /// <summary>
    /// Whether the game is finished or not.
    /// </summary>
    /// <returns>True if finished, false otherwise.</returns>
    bool IsFinished()
    {
        for (int i = 0; i < PlayerNum; i++)
        {
            if (Players[i].IsAlive == false && Players[i].HasBed == false)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Judge the game. Choose a winner or report there is no winner.
    /// </summary>
    void Judge()
    {
        int remainingPlayers = 0;
        for (int i = 0; i < PlayerNum; i++)
        {
            if (Players[i].IsAlive == true || Players[i].HasBed == true)
            {
                remainingPlayers++;
            }
        }

        if (remainingPlayers == 0 || remainingPlayers == PlayerNum)
        {
            Winner = null;
        }
        else
        {
            for (int i = 0; i < PlayerNum; i++)
            {
                if (Players[i].IsAlive == true || Players[i].HasBed == true)
                {
                    Winner = Players[i];
                    break;
                }
            }
        }

        AfterJudgementEvent?.Invoke(this, new AfterJudgementEventArgs(this, Winner));
    }
}
