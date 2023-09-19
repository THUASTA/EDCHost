using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers;

public interface IUpdater
{
    public ICompetitionUpdate? CachedMessage { get; }

    public void StartUpdate();
    public void End();
    public void UpdateCameras(object[]? cameras);
    public void UpdatePlayers(object[]? players);
    public void UpdateInfo(object[]? infos);
    public void UpdateChunks(object[]? chunks);
    public void UpdateMines(object[]? mines);
    public void AddEvent(object newEvent);
}
