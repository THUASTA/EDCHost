namespace EdcHost.ViewerServers.Messages;

public interface IHostConfigurationFromServer : IMessage
{
    public List<int> AvailableCameras { get; }
    public List<object> AvailableSerialPorts { get; }
    public string Message { get; }
}
