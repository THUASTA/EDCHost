using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public interface IHostConfigurationFromServer : IMessage
{
    public List<object> AvailableCameras { get; }
    public List<object> AvailableSerialPorts { get; }
    public string Message { get; }
}
