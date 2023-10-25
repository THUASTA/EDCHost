namespace EdcHost.SlaveServers;

public interface ISerialPortWrapper {
    int BytesToRead { get; }
    void Close();
    void Open();
    void Read(byte[] buffer, int offset, int count);
    void Write(byte[] buffer, int offset, int count);
}
