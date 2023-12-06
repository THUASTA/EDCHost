namespace EdcHost.Games;

partial class Game : IGame
{
    /// <summary>
    /// Raised after game starts.
    /// </summary>
    public event EventHandler<AfterGameStartEventArgs>? AfterGameStartEvent;

    /// <summary>
    /// Raised after a new game tick.
    /// </summary>
    public event EventHandler<AfterGameTickEventArgs>? AfterGameTickEvent;

    /// <summary>
    /// Raised after judgement.
    /// </summary>
    public event EventHandler<AfterJudgementEventArgs>? AfterJudgementEvent;

    /// <summary>
    /// Raised after a player tries to trade.
    /// </summary>
    public event EventHandler<PlayerTradeEventCallbackArgs>? PlayerTradeEventCallback;
}
