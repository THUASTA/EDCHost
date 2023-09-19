using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers;

public class Updater : IUpdater
{
    public ICompetitionUpdate CachedMessage { get; private set; } = new CompetitionUpdate();
    private bool _isRunning = false;
    public event EventHandler<MessageTransferEventArgs>? SendCaller = null;
    private readonly Thread _sendThread;
    private bool _playerUpdate = false;
    private bool _infoUpdate = false;
    private bool _cameraUpdate = false;
    private bool _chunkUpdate = false;
    private bool _mineUpdate = false;

    public Updater()
    {
        _sendThread = new Thread(() =>
        {
            while (_isRunning)
            {
                Task.Delay(47).Wait();

                if (!ReadyToSend())
                    continue;

                SendCaller?.Invoke(this, new MessageTransferEventArgs(CachedMessage));
                Clear();
            }
        });
    }

    public void StartUpdate()
    {
        _isRunning = true;
        Thread.CurrentThread.Name = "SendThread";
        _sendThread.Start();
    }

    public void Clear()
    {
        CachedMessage = new CompetitionUpdate();
        _playerUpdate = false;
        _cameraUpdate = false;
        _chunkUpdate = false;
        _mineUpdate = false;
        _infoUpdate = false;
    }

    public bool ReadyToSend()
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

    public void UpdateInfo(object[]? infos)
    {
        if (infos == null)
        {
            return;
        }
        CachedMessage.Info.Clear();
        foreach (object info in infos)
        {
            CachedMessage.Info.Add(info);
        }
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
