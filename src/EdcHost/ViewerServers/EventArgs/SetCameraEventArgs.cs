namespace EdcHost.ViewerServers.EventArgs;

public class SetCameraEventArgs : System.EventArgs
{
    public int PlayerId { get; }
    public object CameraConfiguration { get; }

    public SetCameraEventArgs(int playerId, object cameraConfiguration)
    {
        PlayerId = playerId;
        CameraConfiguration = cameraConfiguration;
    }
}
