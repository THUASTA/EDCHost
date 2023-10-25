namespace EdcHost.SlaveServers;

public interface ISerialPortHub {
    ISerialPortWrapper Get(string portName);
}
