using EdcHost.ViewerServers.EventArgs;
using Fleck;
using EdcHost.ViewerServers.Messages;
namespace EdcHost.ViewerServers;

/// <summary>
/// ViewerServer handles the API requests from viewers via WebSocket.
/// </summary>
public interface IViewerServer
{
    static IViewerServer Create(int port)
    {
        var webSocketServer = new WebSocketServer($"ws://0.0.0.0:{port}");
        return new ViewerServer(webSocketServer, new GameController());
    }
    IGameController Controller { get; }
    public event EventHandler<SetPortEventArgs>? SetPortEvent;
    public event EventHandler<SetCameraEventArgs>? SetCameraEvent;
    /// <summary>
    /// Starts the server.
    /// </summary>
    void Start();
    /// <summary>
    /// Stops the server.
    /// </summary>
    void Stop();

    void Publish(IMessage message);
}
