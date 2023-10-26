using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Ports;

namespace EdcHost.SlaveServers;

class SerialPortWrapper : ISerialPortWrapper
{
    public event EventHandler<ISerialPortWrapper.AfterReceiveEventArgs>? AfterReceive;

    public string PortName => _serialPort.PortName;

    bool _isOpen = false;
    readonly Serilog.ILogger _logger = Serilog.Log.Logger.ForContext("Component", "SlaveServers");
    readonly ConcurrentQueue<byte[]> _queueOfBytesToSend = new();
    readonly SerialPort _serialPort;
    Task? _taskForReceiving = null;
    Task? _taskForSending = null;

    public SerialPortWrapper(string portName)
    {
        _serialPort = new(portName: portName);
    }

    public int BytesToRead => _serialPort.BytesToRead;

    public void Close() {
        if (!_isOpen){
            throw new InvalidOperationException("port is not open");
        }

        Debug.Assert(_taskForReceiving != null);
        Debug.Assert(_taskForSending != null);

        _isOpen = false;

        _taskForReceiving.Wait();
        _taskForSending.Wait();
        _serialPort.Close();

        _taskForSending.Dispose();
        _taskForReceiving.Dispose();
    }

    public void Dispose() {
        _serialPort.Dispose();
    }

    public void Open() {
        if (_isOpen){
            throw new InvalidOperationException("port is already open");
        }

        _isOpen = true;

        _serialPort.Open();
        _taskForReceiving = Task.Run(TaskForReceivingFunc);
        _taskForSending = Task.Run(TaskForSendingFunc);
    }

    public void Send(byte[] bytes) {
        if (!_isOpen){
            throw new InvalidOperationException("port is not open");
        }

        _queueOfBytesToSend.Enqueue(bytes);
    }

    private async Task TaskForReceivingFunc() {
        while (_isOpen) {
            await Task.Delay(0);

            try {
                if (_serialPort.BytesToRead == 0) {
                    continue;
                }

                byte[] bytes = new byte[_serialPort.BytesToRead];
                _serialPort.Read(bytes, 0, bytes.Length);

                AfterReceive?.Invoke(this, new(_serialPort.PortName, bytes));
            }
            catch (Exception e) {
                _logger.Error(e, "error while sending bytes");
            }
        }
    }

    private async Task TaskForSendingFunc()
    {
        while (_isOpen)
        {
            await Task.Delay(0);

            try {
                if (_queueOfBytesToSend.TryDequeue(out byte[]? bytes))
                {
                    _serialPort.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception e) {
                _logger.Error(e, "error while sending bytes");
            }
        }
    }
}
