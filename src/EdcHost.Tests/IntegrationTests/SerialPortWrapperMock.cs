using EdcHost.SlaveServers;

namespace EdcHost.Tests.IntegrationTests;

class SerialPortWrapperMock : ISerialPortWrapper
{
    public bool IsOpen = false;
    public List<byte> WriteBuffer = new();
    public void MockReceive(byte[] bytes) {
        AfterReceive?.Invoke(this, new ISerialPortWrapper.AfterReceiveEventArgs(PortName, bytes));
    }

    public event EventHandler<ISerialPortWrapper.AfterReceiveEventArgs>? AfterReceive;

    public string PortName {get;}

    public SerialPortWrapperMock(string portName)
    {
        PortName = portName;
    }

    public void Close()
    {
        IsOpen = false;
    }
    public void Dispose()
    {
    }
    public void Open()
    {
        if (IsOpen)
        {
            throw new InvalidOperationException();
        }

        IsOpen = true;
    }
    public void Send(byte[] bytes)
    {
        if (!IsOpen)
        {
            throw new InvalidOperationException();
        }

        WriteBuffer.Clear();
        WriteBuffer.AddRange(bytes);
    }
}
