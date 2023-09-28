using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public class Error : IError
{
    [JsonPropertyName("messageType")]
    public string MessageType { get; }

    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonConstructor]
    public Error(string messageType, int errorCode, string message)
        => (MessageType, ErrorCode, Message) = (messageType, errorCode, message);

    public Error(int errorCode, string message)
        => (MessageType, ErrorCode, Message) = (new string("ERROR"), errorCode, message);

    public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this);

    public string SerializeToString() => JsonSerializer.Serialize(this);
}
