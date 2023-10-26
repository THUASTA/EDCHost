namespace EdcHost.ViewerServers.Messages;

public interface ICompetitionUpdate : IMessage
{
    public List<object> Cameras { get; }
    public List<object> Chunks { get; }
    public List<object> Events { get; }
    public object Info { get; set; }
    public List<object> Mines { get; }
    public List<object> Players { get; }

    
}
