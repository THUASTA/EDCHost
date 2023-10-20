namespace EdcHost.ViewerServers.Messages;

public interface ICompetitionControlCommand : IMessage
{
    public string Token { get; }
    public string Command { get; }
}
