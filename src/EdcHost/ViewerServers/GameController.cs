using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers;

public class GameController : IGameController
{
    public event EventHandler? StartGameEvent;
    public event EventHandler? EndGameEvent;
    public event EventHandler? ResetGameEvent;
    public event EventHandler<MessageTransferEventArgs>? GetHostConfigurationEvent;
    private List<object>? _availablePorts;
    private List<int>? _availableCameras;

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame()
    {
        StartGameEvent?.Invoke(this, System.EventArgs.Empty);
    }
    /// <summary>
    /// End the game.
    /// </summary>
    public void EndGame()
    {
        EndGameEvent?.Invoke(this, System.EventArgs.Empty);
    }
    /// <summary>
    /// Reset the game.
    /// </summary>
    public void ResetGame()
    {
        ResetGameEvent?.Invoke(this, System.EventArgs.Empty);
    }
    /// <summary>
    /// Get host configuration.
    /// </summary>
    public void GetHostConfiguration()
    {
        if (_availablePorts == null || _availableCameras == null)
        {
            throw new Exception("Ports or cameras are not set.");
        }
        GetHostConfigurationEvent?.Invoke(
            this,
            new MessageTransferEventArgs(
                new HostConfigurationFromServer(
                    _availableCameras,
                    _availablePorts
                )
            )
        );
    }
    /// <summary>
    /// Set ports and cameras.
    /// </summary>
    /// <param name="ports">names of ports. eg: "COM3"</param>
    /// <param name="cameras">names of cameras.</param>
    public void SetAvailableDevice(string[] ports, int[] cameras)
    {
        _availablePorts = new List<object>(ports.ToList<object>());
        _availableCameras = new List<int>(cameras.ToList<int>());
    }
}
