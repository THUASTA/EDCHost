using EdcHost.Games;
using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers.EventArgs;

public class SetCameraEventArgs : System.EventArgs
{
    public int PlayerId { get; }
    public object CameraConfiguration { get; }

    public SetCameraEventArgs(int playerId, PlayerConfiguration.CameraInfo cameraConfiguration)
    {
        PlayerId = playerId;
        CameraConfiguration = cameraConfiguration;
    }
}
