using EdcHost.ViewerServers.EventArgs;

namespace EdcHost.ViewerServers;

public interface IGameController
{
    public event EventHandler? StartGameEvent;
    public event EventHandler? EndGameEvent;
    public event EventHandler? ResetGameEvent;
    public event EventHandler<MessageTransferEventArgs>? GetHostConfigurationEvent;

    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartGame();
    /// <summary>
    /// End the game.
    /// </summary>
    public void EndGame();
    /// <summary>
    /// Reset the game.
    /// </summary>
    public void ResetGame();
    /// <summary>
    /// Get host configuration.
    /// </summary>
    public void GetHostConfiguration();
    /// <summary>
    /// Set ports and cameras.
    /// </summary>
    /// <param name="ports">names of ports. eg: "COM3"</param>
    /// <param name="cameras">names of cameras.</param>
    public void SetAvailableDevices(string[] ports, int[] cameras);
}
