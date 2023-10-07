using EdcHost.Games;
using EdcHost.SlaveServers;
using EdcHost.SlaveServers.EventArgs;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    /// <remarks>
    /// These handlers handle events about players.
    /// To implement, call functions in _game.Players[playerId].
    /// </remarks>
    private void HandlePlayerTryAttackEvent(object? sender, PlayerTryAttackEventArgs e)
    {
        //TODO: Implement
    }

    private void HandlePlayerTryUseEvent(object? sender, PlayerTryUseEventArgs e)
    {
        //TODO: Implement
    }

    private void HandlePlayerTryTradeEvent(object? sender, PlayerTryTradeEventArgs e)
    {
        //TODO: Implement
    }
}
