using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using EdcHost.SlaveServers.EventArgs;
using Serilog;

namespace EdcHost.SlaveServers;

public class SlaveServer : ISlaveServer
{
    record PortComponentBundle
    {
        public ConcurrentQueue<IPacketFromHost> PacketsToSend;
        public ConcurrentQueue<IPacketFromSlave> PacketsReceived;
        public SerialPort SerialPort;
        public bool ShouldRun = false;
        public Task TaskForSending;
        public Task TaskForReceiving;

        public PortComponentBundle(SerialPort serialPort, Task taskForSending,
            Task taskForReceiving, ConcurrentQueue<IPacketFromHost> packetsToSend,
            ConcurrentQueue<IPacketFromSlave> packetsReceived)
        {
            SerialPort = serialPort;
            TaskForSending = taskForSending;
            TaskForReceiving = taskForReceiving;
            PacketsToSend = packetsToSend;
            PacketsReceived = packetsReceived;
        }
    }

    public event EventHandler<PlayerTryAttackEventArgs>? PlayerTryAttackEvent;
    public event EventHandler<PlayerTryUseEventArgs>? PlayerTryUseEvent;
    public event EventHandler<PlayerTryTradeEventArgs>? PlayerTryTradeEvent;

    readonly ConcurrentDictionary<string, PortComponentBundle> _portComponentBundles = new();
    readonly ILogger _logger = Log.Logger.ForContext("Component", "SlaveServers");

    public void AddPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    {
        if (_portComponentBundles.Keys.Any(portName.Equals))
        {
            throw new ArgumentException($"port name already exists: {portName}");
        }

        SerialPort serialPort = new(portName: portName, baudRate: baudRate, parity: parity,
            dataBits: dataBits, stopBits: stopBits);

        ConcurrentQueue<IPacketFromHost> packetsToSend = new();
        ConcurrentQueue<IPacketFromSlave> packetsReceived = new();

        Task taskForSending = new(() => SendTaskFunc(portName));
        Task taskForReceiving = new(() => ReceiveTaskFunc(portName));

        _portComponentBundles.TryAdd(portName, new PortComponentBundle(serialPort, taskForSending,
            taskForReceiving, packetsToSend, packetsReceived));
    }

    public void RemovePort(string portName)
    {
        if (!_portComponentBundles.Keys.Any(portName.Equals))
        {
            throw new ArgumentException($"port name does not exist: {portName}");
        }

        _portComponentBundles[portName].ShouldRun = false;
        _portComponentBundles[portName].TaskForSending.Wait();
        _portComponentBundles[portName].TaskForReceiving.Wait();
        _portComponentBundles[portName].SerialPort.Close();
        _portComponentBundles.Remove(portName, out _);
    }

    public void Publish(string portName, int gameStage, int elapsedTime, List<int> heightOfChunks,
        bool hasBed, float positionX, float positionY, float positionOpponentX,
        float positionOpponentY, int agility, int health, int maxHealth, int strength,
        int emeraldCount, int woolCount)
    {
        if (!_portComponentBundles.Keys.Any(portName.Equals))
        {
            throw new ArgumentException($"port name does not exist: {portName}");
        }

        _portComponentBundles[portName].PacketsToSend.Enqueue(new PacketFromHost(gameStage, elapsedTime, heightOfChunks,
            hasBed, positionX, positionY, positionOpponentX, positionOpponentY, agility, health, maxHealth, strength,
            emeraldCount, woolCount));
    }

    public void Start()
    {
        _logger.Information("Starting...");

        foreach (PortComponentBundle portComponentBundle in _portComponentBundles.Values)
        {
            portComponentBundle.ShouldRun = true;

            portComponentBundle.SerialPort.Open();
            portComponentBundle.TaskForSending.Start();
            portComponentBundle.TaskForReceiving.Start();
        }

        _logger.Information("Started.");
    }

    public void Stop()
    {
        _logger.Information("Stopping...");

        foreach (PortComponentBundle portComponentBundle in _portComponentBundles.Values)
        {
            portComponentBundle.ShouldRun = false;

            portComponentBundle.TaskForSending.Wait();
            portComponentBundle.TaskForReceiving.Wait();
            portComponentBundle.SerialPort.Close();
        }

        _logger.Information("Stopped.");
    }

    void PerformAction(string portName, IPacketFromSlave packet)
    {
        switch (packet.ActionType)
        {
            case (int)ActionTypes.Attack:
                if (Enum.IsDefined(typeof(Directions), packet.Param))
                {
                    PlayerTryAttackEvent?.Invoke(this, new PlayerTryAttackEventArgs(portName, packet.Param));
                }
                break;

            case (int)ActionTypes.Use:
                if (Enum.IsDefined(typeof(Directions), packet.Param))
                {
                    PlayerTryUseEvent?.Invoke(this, new PlayerTryUseEventArgs(portName, packet.Param));
                }
                break;

            case (int)ActionTypes.Trade:
                if (Enum.IsDefined(typeof(ItemList), packet.Param))
                {
                    PlayerTryTradeEvent?.Invoke(this, new PlayerTryTradeEventArgs(portName, packet.Param));
                }
                break;

            default:
                break;
        }
    }

    void SendTaskFunc(string portName)
    {
        while (_portComponentBundles.GetValueOrDefault(portName)?.ShouldRun ?? false)
        {
            try
            {
                if (_portComponentBundles[portName].PacketsToSend.TryDequeue(out IPacketFromHost? packet))
                {
                    byte[] message = packet.ToBytes();
                    _portComponentBundles[portName].SerialPort.Write(message, 0, message.Length);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "error while sending packet");
            }
        }
    }

    void ReceiveTaskFunc(string portName)
    {
        while (_portComponentBundles.GetValueOrDefault(portName)?.ShouldRun ?? false)
        {
            try
            {
                if (_portComponentBundles[portName].SerialPort.BytesToRead == 0)
                {
                    continue;
                }
                byte[] message = new byte[_portComponentBundles[portName].SerialPort.BytesToRead];
                _portComponentBundles[portName].SerialPort.Read(message, 0, message.Length);
                _portComponentBundles[portName].PacketsReceived.Enqueue(new PacketFromSlave(message));

                while (_portComponentBundles[portName].PacketsReceived.TryDequeue(out IPacketFromSlave? packet))
                {
                    PerformAction(portName, packet);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "error while receiving packet");
            }
        }
    }
}
