namespace EdcHost.ViewerServers.Messages;

public interface IError : IMessage
{
    public int ErrorCode { get; }
    public string Message { get; }
}
