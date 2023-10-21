using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers;

public class Updater : IUpdater
{
    public ICompetitionUpdate CachedMessage { get; private set; } = new CompetitionUpdate();
    bool _isRunning = false;
    public event EventHandler<MessageTransferEventArgs>? SendEvent;
    readonly Thread _sendThread;
    bool _playerUpdate = false;
    bool _infoUpdate = false;
    bool _cameraUpdate = false;
    bool _chunkUpdate = false;
    bool _mineUpdate = false;

    public Updater()
    {
        _sendThread = new Thread(() =>
        {
            while (_isRunning)
            {
                Task.Delay(50).Wait();

                if (!ReadyToSend())
                    continue;

                SendEvent?.Invoke(this, new MessageTransferEventArgs(CachedMessage));
                Clear();
            }
        });
    }

    public void StartUpdate()
    {
        _isRunning = true;
        _sendThread.Start();
    }

    void Clear()
    {
        CachedMessage = new CompetitionUpdate();
        _playerUpdate = false;
        _cameraUpdate = false;
        _chunkUpdate = false;
        _mineUpdate = false;
        _infoUpdate = false;
    }

    bool ReadyToSend()
    {
        return _playerUpdate && _cameraUpdate && _chunkUpdate && _mineUpdate && _infoUpdate;
    }

    public void UpdateCameras(object[]? cameras)
    {
        if (cameras == null)
        {
            return;
        }
        CachedMessage.Cameras.Clear();
        foreach (object camera in cameras)
        {
            CachedMessage.Cameras.Add(camera);
        }
        _cameraUpdate = true;
    }

    public void UpdatePlayers(object[]? players)
    {
        if (players == null)
        {
            return;
        }
        CachedMessage.Players.Clear();
        foreach (object player in players)
        {
            CachedMessage.Players.Add(player);
        }
        _playerUpdate = true;
    }

    public void UpdateInfo(object? info)
    {
        if (info == null)
        {
            return;
        }
        CachedMessage.Info = info;
        _infoUpdate = true;
    }

    public void UpdateChunks(object[]? chunks)
    {
        if (chunks == null)
        {
            return;
        }
        CachedMessage.Chunks.Clear();
        foreach (object chunk in chunks)
        {
            CachedMessage.Chunks.Add(chunk);
        }
        _chunkUpdate = true;
    }

    public void UpdateMines(object[]? mines)
    {
        if (mines == null)
        {
            return;
        }
        CachedMessage.Mines.Clear();
        foreach (object mine in mines)
        {
            CachedMessage.Mines.Add(mine);
        }
        _mineUpdate = true;
    }

    public void AddEvent(object newEvent)
    {
        CachedMessage.Events.Add(newEvent);
    }

    public void End()
    {
        _isRunning = false;
    }
}
