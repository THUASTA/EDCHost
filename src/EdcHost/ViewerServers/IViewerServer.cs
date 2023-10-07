using EdcHost.ViewerServers.EventArgs;

namespace EdcHost.ViewerServers;

/// <summary>
/// ViewerServer handles the API requests from viewers via WebSocket.
/// </summary>
public interface IViewerServer
{
    public IUpdater CompetitionUpdater { get; }
    public IGameController Controller { get; }
    public event EventHandler<SetPortEventArgs>? SetPortEvent;
    public event EventHandler<SetCameraEventArgs>? SetCameraEvent;

    /// <summary>
    /// Starts the server.
    /// </summary>
    public void Start();
    /// <summary>
    /// Stops the server.
    /// </summary>
    public void Stop();
    /// <summary>
    /// Raise an error to the viewer.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    public void RaiseError(int errorCode, string message);
}
