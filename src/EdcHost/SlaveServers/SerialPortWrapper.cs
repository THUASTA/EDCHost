using System.IO.Ports;

namespace EdcHost.SlaveServers;

class SerialPortWrapper: ISerialPortWrapper {
    readonly SerialPort _serialPort;

    public SerialPortWrapper(string portName) {
        _serialPort = new(portName: portName);
    }

    public int BytesToRead => _serialPort.BytesToRead;

    public void Close() => _serialPort.Close();

    public void Open() => _serialPort.Open();

    public void Read(byte[] buffer, int offset, int count) => _serialPort.Read(buffer, offset, count);

    public void Write(byte[] buffer, int offset, int count) => _serialPort.Write(buffer, offset, count);
}
