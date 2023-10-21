namespace EdcHost.ViewerServers.Messages;

public interface IHostConfigurationFromClient : IMessage
{
    public string Token { get; }
    public List<object> Players { get; }
}
