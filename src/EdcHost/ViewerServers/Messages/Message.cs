using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public class Message : IMessage
{
    [JsonPropertyName("messageType")]
    public string MessageType { get; }

    [JsonConstructor]
    public Message(string messageType)
        => MessageType = messageType;

    public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this);

    public string SerializeToString() => JsonSerializer.Serialize(this);
}
