namespace EdcHost;

/// <summary>
/// EdcHost does all the work of the program.
/// </summary>
public interface IEdcHost
{
    public static IEdcHost Create(EdcHostOptions options)
    {
        Games.Game game = new();
        SlaveServers.SlaveServer slaveServer = new(new string[] { }, new int[] { });
        ViewerServers.ViewerServer viewerServer = new(options.ServerPort);

        return new EdcHost(
            game: game,
            slaveServer: slaveServer,
            viewerServer: viewerServer
        );
    }

    /// <summary>
    /// Starts the host.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stops the host.
    /// </summary>
    public void Stop();
}
