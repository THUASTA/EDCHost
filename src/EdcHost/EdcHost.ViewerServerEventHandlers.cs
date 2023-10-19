using EdcHost.ViewerServers.EventArgs;

namespace EdcHost;

public partial class EdcHost : IEdcHost
{
    private void HandleSetPortEvent(object? sender, SetPortEventArgs e)
    {
        //TODO: Set port name and baud rate

        Serilog.Log.Information("[Update]");
        Serilog.Log.Information($"Player {e.PlayerId}:");
        Serilog.Log.Information($"Port: {e.PortName} BaudRate: {e.BaudRate}");
    }

    private void HandleSetCameraEvent(object? sender, SetCameraEventArgs e)
    {
        //TODO: Set camera

        Serilog.Log.Information("[Update]");
        Serilog.Log.Information($"Player {e.PlayerId}:");
        Serilog.Log.Information($"Camera: {e.CameraConfiguration}");
    }

    private void HandleStartGameEvent(object? sender, EventArgs e)
    {
        try
        {
            _game.Start();
            Serilog.Log.Information("Game started.");
        }
        catch (Exception exception)
        {
            Serilog.Log.Error($"Failed to start game: {exception}");
        }
    }

    private void HandleStopGameEvent(object? sender, EventArgs e)
    {
        try
        {
            _game.Stop();
            Serilog.Log.Information("Game stpped.");
        }
        catch (Exception exception)
        {
            Serilog.Log.Error($"Failed to stop game: {exception}");
        }
    }

    private void HandleResetGameEvent(object? sender, EventArgs e)
    {
        try
        {
            //TODO: Reset game
        }
        catch (Exception exception)
        {
            Serilog.Log.Error($"Failed to reset game: {exception}");
        }
    }
}
