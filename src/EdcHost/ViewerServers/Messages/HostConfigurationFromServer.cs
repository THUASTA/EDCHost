using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public class HostConfigurationFromServer : IHostConfigurationFromServer
{
    [JsonPropertyName("messageType")]
    public string MessageType { get; }

    [JsonPropertyName("availableCameras")]
    public List<object> AvailableCameras { get; }

    [JsonPropertyName("availableSerialPorts")]
    public List<object> AvailableSerialPorts { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonConstructor]
    public HostConfigurationFromServer(
        string messageType,
        List<object> availableCameras,
        List<object> availableSerialPorts,
        string message
    ) => (MessageType, AvailableCameras, AvailableSerialPorts, Message)
        = (messageType, availableCameras, availableSerialPorts, message);

    public HostConfigurationFromServer(
        List<object> availableCameras,
        List<object> availableSerialPorts,
        string message = "device info"
    ) => (MessageType, AvailableCameras, AvailableSerialPorts, Message)
        = (new string("HOST_CONFIGURATION_FROM_SERVER"), availableCameras, availableSerialPorts, message);

    public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this);

    public string SerializeToString() => JsonSerializer.Serialize(this);
}
