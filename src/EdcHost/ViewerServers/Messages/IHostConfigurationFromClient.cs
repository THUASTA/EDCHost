using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public interface IHostConfigurationFromClient : IMessage
{
    public string Token { get; }
    public List<PlayerConfiguration> Players { get; }
}