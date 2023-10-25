namespace EdcHost.SlaveServers;

class SerialPortHub: ISerialPortHub {
    public ISerialPortWrapper Get(string portName) => new SerialPortWrapper(portName);
}
