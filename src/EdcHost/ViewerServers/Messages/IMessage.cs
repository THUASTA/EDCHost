namespace EdcHost.ViewerServers.Messages;

/// <summary>
/// Store information of game status.
/// </summary>
public interface IMessage
{
    public string MessageType { get; }

    public byte[] SerializeToUtf8Bytes();

    public string SerializeToString();
}
