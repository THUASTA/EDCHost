using System.IO.Ports;
using EdcHost.SlaveServers.EventArgs;

namespace EdcHost.SlaveServers;

/// <summary>
/// SlaveServer handles the communication with the slaves via UART.
/// </summary>
public interface ISlaveServer
{
    event EventHandler<PlayerTryAttackEventArgs>? PlayerTryAttackEvent;
    event EventHandler<PlayerTryUseEventArgs>? PlayerTryUseEvent;
    event EventHandler<PlayerTryTradeEventArgs>? PlayerTryTradeEvent;

    void AddPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits);

    void RemovePort(string portName);

    void Publish(string portName, int gameStage, int elapsedTime, List<int> heightOfChunks,
        bool hasBed, float positionX, float positionY, float positionOpponentX,
        float positionOpponentY, int agility, int health, int maxHealth, int strength,
        int emeraldCount, int woolCount);

    /// <summary>
    /// Starts the server.
    /// </summary>
    void Start();

    /// <summary>
    /// Stops the server.
    /// </summary>
    public void Stop();

    public void UpdatePacket(int id, IPacket packet);

    public void SetPortName(int id, string portName);

    public void SetPortBaudRate(int id, int baudRate);
}
