using EdcHost.SlaveServers.EventArgs;

namespace EdcHost.SlaveServers;

/// <summary>
/// SlaveServer handles the communication with the slaves via UART.
/// </summary>
public interface ISlaveServer
{
    public event EventHandler<PlayerTryAttackEventArgs>? PlayerTryAttackEvent;
    public event EventHandler<PlayerTryUseEventArgs>? PlayerTryUseEvent;
    public event EventHandler<PlayerTryTradeEventArgs>? PlayerTryTradeEvent;

    /// <summary>
    /// Starts the server.
    /// </summary>
    public void Start();
    /// <summary>
    /// Stops the server.
    /// </summary>
    public void Stop();

    public void UpdatePacket(int id, IPacket packet);
}
