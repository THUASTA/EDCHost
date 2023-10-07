namespace EdcHost.Games;

/// <summary>
/// Game handles the game logic.
/// </summary>
public partial class Game : IGame
{
    /// <summary>
    /// When will Battling stage start.
    /// </summary>
    public readonly TimeSpan StartBattlingTime = TimeSpan.FromSeconds(600);

    /// <summary>
    /// How much time a player should wait until respawn.
    /// </summary>
    public readonly TimeSpan RespawnTimeInterval = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Time interval between two battling damages.
    /// </summary>
    public readonly TimeSpan BattlingDamageInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Time interval between two ticks.
    /// </summary>
    private readonly TimeSpan TickInterval = TimeSpan.FromSeconds(0.05d);

    /// <summary>
    /// Current stage of the game.
    /// </summary>
    public IGame.Stage CurrentStage { get; private set; }

    /// <summary>
    /// Elapsed time of the game.
    /// </summary>
    public TimeSpan ElapsedTime { get; private set; }

    /// <summary>
    /// Current tick of game.
    /// </summary>
    public int CurrentTick { get; private set; }

    /// <summary>
    /// Winner of the game.
    /// </summary>
    /// <remarks>
    /// Winner can be null in case there is no winner.
    /// </remarks>
    public IPlayer? Winner { get; private set; }

    /// <summary>
    /// The game map.
    /// </summary>
    public IMap GameMap { get; private set; }

    /// <summary>
    /// All mines.
    /// </summary>
    public List<IMine> Mines { get; private set; }

    /// <summary>
    /// When game starts.
    /// </summary>
    private DateTime? _startTime;

    /// <summary>
    /// Last tick.
    /// </summary>
    private DateTime? _lastTickTime;

    /// <summary>
    /// Last time when a battling damage is dealt.
    /// </summary>
    private DateTime? _lastBattlingDamageTime;

    /// <summary>
    /// The tick task.
    /// </summary>
    private readonly Task _tickTask;

    /// <summary>
    /// Construct a Game object.
    /// </summary>
    public Game()
    {
        CurrentStage = IGame.Stage.Ready;
        ElapsedTime = TimeSpan.FromSeconds(0);
        Winner = null;
        CurrentTick = 0;

        _startTime = null;
        _lastTickTime = null;
        _lastBattlingDamageTime = null;

        GameMap = new Map(new IPosition<int>[] { new Position<int>(0, 0), new Position<int>(7, 7) });

        Players = new();

        _playerLastAttackTime = new();
        for (int i = 0; i < 2; i++)
        {
            _playerLastAttackTime.Add(DateTime.Now - TimeSpan.FromSeconds(20));
        }

        _playerDeathTime = new();
        for (int i = 0; i < 2; i++)
        {
            _playerDeathTime.Add(null);
        }

        Mines = new();
        GenerateMines();

        _allBedsDestroyed = false;

        _tickTask = new(Tick);
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void Start()
    {
        if (_startTime is not null)
        {
            throw new InvalidOperationException("The game is already started.");
        }

        Players.Clear();

        //TODO: Set player's initial position and spawnpoint

        Players.Add(new Player(0, 0.4f, 0.4f, 0.4f, 0.4f));
        Players.Add(new Player(1, 7.4f, 7.4f, 7.4f, 7.4f));

        for (int i = 0; i < 2; i++)
        {
            Players[i].OnMove += HandlePlayerMoveEvent;
            Players[i].OnAttack += HandlePlayerAttackEvent;
            Players[i].OnPlace += HandlePlayerPlaceEvent;
            Players[i].OnDie += HandlePlayerDieEvent;
        }

        for (int i = 0; i < 2; i++)
        {
            _playerLastAttackTime[i] = DateTime.Now - TimeSpan.FromSeconds(20);
        }

        for (int i = 0; i < 2; i++)
        {
            _playerDeathTime[i] = null;
        }

        //TODO: Start game after all players are ready

        foreach (IMine mine in Mines)
        {
            mine.GenerateOre();
        }

        CurrentStage = IGame.Stage.Running;
        Winner = null;
        CurrentTick = 0;

        DateTime initTime = DateTime.Now;
        _startTime = initTime;
        _lastTickTime = initTime;
        ElapsedTime = TimeSpan.FromSeconds(0);

        _lastBattlingDamageTime = null;

        _allBedsDestroyed = false;

        _tickTask.Start();

        AfterGameStartEvent?.Invoke(this, new AfterGameStartEventArgs(this, _startTime));
    }

    /// <summary>
    /// Stops the game.
    /// </summary>
    public void Stop()
    {
        if (_startTime is null)
        {
            Serilog.Log.Warning("The game has not started yet.");
        }

        lock (this)
        {
            _startTime = null;
            _lastTickTime = null;
            _lastBattlingDamageTime = null;

            Players.Clear();

            for (int i = 0; i < 2; i++)
            {
                _playerLastAttackTime[i] = DateTime.Now - TimeSpan.FromSeconds(20);
            }

            for (int i = 0; i < 2; i++)
            {
                _playerDeathTime[i] = null;
            }

            foreach (IMine mine in Mines)
            {
                mine.PickUpOre(mine.AccumulatedOreCount);
            }

            ElapsedTime = TimeSpan.FromSeconds(0);
            CurrentTick = 0;

            _allBedsDestroyed = false;
        }

        Serilog.Log.Information("Game stopped.");
    }

    /// <summary>
    /// Ticks the game.
    /// </summary>
    public void Tick()
    {
        while (_startTime is not null)
        {
            try
            {
                lock (this)
                {
                    DateTime currentTime = DateTime.Now;
                    ElapsedTime = currentTime - (DateTime)_startTime;

                    if (CurrentStage == IGame.Stage.Finished)
                    {
                        Judge();
                        break;
                    }

                    if (CurrentStage == IGame.Stage.Battling)
                    {
                        if (_allBedsDestroyed == false)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Players[i].DestroyBed();
                            }
                            _allBedsDestroyed = true;
                        }

                        if (_lastBattlingDamageTime is null
                            || DateTime.Now - _lastBattlingDamageTime > BattlingDamageInterval)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Players[i].Hurt(1);
                            }
                            _lastBattlingDamageTime = DateTime.Now;
                        }
                    }

                    Update();

                    if (currentTime - _lastTickTime >= TickInterval)
                    {

                        _lastTickTime = currentTime;
                        CurrentTick++;

                        AfterGameTickEvent?.Invoke(
                            this, new AfterGameTickEventArgs(this, CurrentTick));
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error($"An exception has been caught: {e}");
            }
        }
    }

    /// <summary>
    /// Generate mines when game start.
    /// </summary>
    private void GenerateMines()
    {
        //TODO: Generate mines according to game rule
    }

    /// <summary>
    /// Whether all players are ready.
    /// </summary>
    /// <returns>True if all reasy, false otherwise.</returns>
    private bool IsReady()
    {
        //TODO: Check whether all players are ready
        return true;
    }

    /// <summary>
    /// Update game.
    /// </summary>
    private void Update()
    {
        if (_startTime is null)
        {
            throw new InvalidOperationException("The game is not running.");
        }

        UpdatePlayerInfo();
        UpdateMines();
        UpdateGameStage();
    }

    /// <summary>
    /// Update game stage.
    /// </summary>
    private void UpdateGameStage()
    {
        if (IsFinished())
        {
            CurrentStage = IGame.Stage.Finished;
        }
        else if (ElapsedTime >= StartBattlingTime)
        {
            CurrentStage = IGame.Stage.Battling;
        }
        else
        {
            CurrentStage = IGame.Stage.Running;
        }
    }

    /// <summary>
    /// Update player infomation.
    /// </summary>
    /// <remarks>
    /// Similar things are done by player event handlers.
    /// But this function is still called in case there are
    /// something player event handlers can't do.
    /// </remarks>
    private void UpdatePlayerInfo()
    {
        for (int i = 0; i < 2; i++)
        {
            if (Players[i].HasBed == true
                && GameMap.GetChunkAt(ToIntPosition(Players[i].SpawnPoint)).IsVoid == true)
            {
                Players[i].DestroyBed();
            }

            if (Players[i].IsAlive == false && Players[i].HasBed == true
                && DateTime.Now - _playerDeathTime[i] > RespawnTimeInterval
                && IsSamePosition(
                    ToIntPosition(Players[i].PlayerPosition), ToIntPosition(Players[i].SpawnPoint)
                    ) == true)
            {
                Players[i].Spawn(Players[i].MaxHealth);
                _playerDeathTime[i] = null;

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
    private void UpdateMines()
    {
        DateTime currentTime = DateTime.Now;
        foreach (IMine mine in Mines)
        {
            if (currentTime - mine.LastOreGeneratedTime >= mine.AccumulateOreInterval)
            {
                mine.GenerateOre();
            }
            for (int i = 0; i < 2; i++)
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
    private bool IsFinished()
    {
        for (int i = 0; i < 2; i++)
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
    private void Judge()
    {
        int remainingPlayers = 0;
        for (int i = 0; i < 2; i++)
        {
            if (Players[i].IsAlive == true || Players[i].HasBed == true)
            {
                remainingPlayers++;
            }
        }

        if (remainingPlayers == 0 || remainingPlayers == 2)
        {
            Winner = null;
        }
        else
        {
            for (int i = 0; i < 2; i++)
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
