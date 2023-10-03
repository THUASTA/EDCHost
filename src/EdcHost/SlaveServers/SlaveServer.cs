using System.IO.Ports;

using EdcHost.SlaveServers.EventArgs;
using Fleck;
using Serilog;

namespace EdcHost.SlaveServers;

public class SlaveServer : ISlaveServer
{
    public const int PLAYER_NUM = 2;
    public static readonly int[] BaudRateList = { 9600, 19200, 38400, 57600, 115200 };
    private readonly SerialPort[] _serialPorts;
    private readonly Thread _sendThread;
    private readonly Thread _receiveThread;
    private bool _isRunning = false;
    private readonly IPacket?[] _packetsToSend = { null, null };
    private readonly IPacketFromSlave[] _packetsReceived = new IPacketFromSlave[PLAYER_NUM];
    private readonly ILogger _logger = Log.Logger.ForContext<SlaveServer>();
    public event EventHandler<PlayerTryAttackEventArgs>? PlayerTryAttackEvent;
    public event EventHandler<PlayerTryUseEventArgs>? PlayerTryUseEvent;
    public event EventHandler<PlayerTryTradeEventArgs>? PlayerTryTradeEvent;

    public SlaveServer(string[] portNames, int[] baudRates)
    {
        _serialPorts = new SerialPort[PLAYER_NUM];
        if (portNames.Length != PLAYER_NUM
            || baudRates.Length != PLAYER_NUM)
        {
            throw new ArgumentException($"portNameList, BaudRateList, ParityList, DataBitsList, StopBitsList must have length {PLAYER_NUM}");
        }

        for (int i = 0; i < PLAYER_NUM; i++)
        {
            _serialPorts[i] = new SerialPort();
            SetPortName(i, portNames[i]);
            SetPortBaudRate(i, baudRates[i]);
        }

        _sendThread = new Thread(Send);
        _receiveThread = new Thread(Receive);
    }

    public void Start()
    {
        _isRunning = true;
        for (int i = 0; i < 2; i++)
        {
            _serialPorts[i].Open();
        }
        _sendThread.Start();
        _receiveThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
        _sendThread.Join();
        _receiveThread.Join();
        for (int i = 0; i < 2; i++)
        {
            _serialPorts[i].Close();
        }
    }

    public void UpdatePacket(int id, IPacket packet)
    {
        _packetsToSend[id] = packet;
    }

    public void Send()
    {
        while (_isRunning)
        {
            Task.Delay(100).Wait();
            for (int i = 0; i < 2; i++)
            {
                if (_packetsToSend[i] != null)
                {
                    byte[] message = _packetsToSend[i]?.MakePacket() ?? throw new NullReferenceException();
                    _serialPorts[i].Write(message, 0, message.Length);
                }
            }
        }
    }

    public void Receive()
    {
        while (_isRunning)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_serialPorts[i].BytesToRead > 0)
                {
                    byte[] message = new byte[_serialPorts[i].BytesToRead];
                    _serialPorts[i].Read(message, 0, message.Length);
                    _packetsReceived[i]?.ExtractPacketData(message);

                    //Execute the event
                    PerformAction(i, _packetsReceived[i]);
                }
            }
        }
    }

    private void SetPortName(int id, string portName)
    {
        string[] ports = SerialPort.GetPortNames();
        if (ports.Contains(portName))
        {
            _serialPorts[id].PortName = portName;
        }
        else
        {
            _logger.Error("Port name {portName} is not available.", portName);
        }
    }

    private void SetPortBaudRate(int id, int baudRate)
    {
        foreach (int availableRate in BaudRateList)
        {
            if (baudRate == availableRate)
            {
                _serialPorts[id].BaudRate = baudRate;
                break;
            }
            else
            {
                throw new ArgumentException(
                    "Baud rate must be one of the following: " + string.Join(", ", BaudRateList));
            }
        }
    }

    private void SetPortParity(int id, Parity parity)
    {
        _serialPorts[id].Parity = parity;
    }

    private void SetPortDataBits(int id, int dataBits)
    {
        if (dataBits >= 5 && dataBits <= 8)
        {
            _serialPorts[id].DataBits = dataBits;
        }
        else
        {
            throw new ArgumentException("Data bits must be between 5 and 8");
        }
    }

    private void SetStopBits(int id, StopBits stopBits)
    {
        _serialPorts[id].StopBits = stopBits;
    }

    private void PerformAction(int id, IPacketFromSlave packet)
    {
        switch (packet.ActionType)
        {
            case (int)ActionTypes.Attack:
                if (Enum.IsDefined(typeof(Directions), packet.Param))
                {
                    PlayerTryAttackEvent?.Invoke(this, new PlayerTryAttackEventArgs(id, packet.Param));
                }
                break;

            case (int)ActionTypes.Use:
                if (Enum.IsDefined(typeof(Directions), packet.Param))
                {
                    PlayerTryUseEvent?.Invoke(this, new PlayerTryUseEventArgs(id, packet.Param));
                }
                break;

            case (int)ActionTypes.Trade:
                if (Enum.IsDefined(typeof(ItemList), packet.Param))
                {
                    PlayerTryTradeEvent?.Invoke(this, new PlayerTryTradeEventArgs(id, packet.Param));
                }
                break;

            default:
                break;
        }
    }
}
