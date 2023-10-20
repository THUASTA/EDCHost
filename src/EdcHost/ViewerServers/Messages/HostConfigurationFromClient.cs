using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public class HostConfigurationFromClient : IHostConfigurationFromClient
{
    // The message type of the command
    [JsonPropertyName("messageType")]
    public string MessageType { get; private set; }

    [JsonPropertyName("token")]
    public string Token { get; private set; }

    [JsonPropertyName("players")]
    public List<object> Players { get; private set; }

    [JsonConstructor]
    public HostConfigurationFromClient(string messageType, string token, List<object> players)
        => (MessageType, Token, Players) = (messageType, token, players);

    public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this);

    public string SerializeToString() => JsonSerializer.Serialize(this);
}
