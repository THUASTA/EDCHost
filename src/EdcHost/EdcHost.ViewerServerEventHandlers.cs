using EdcHost.ViewerServers.EventArgs;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandleSetPortEvent(object? sender, SetPortEventArgs e)
    {
        try
        {
            //TODO: Update port

            Serilog.Log.Information("[Update]");
            Serilog.Log.Information($"Player {e.PlayerId}:");
            Serilog.Log.Information($"Port: {e.PortName} BaudRate: {e.BaudRate}");
        }
        catch (Exception exception) {
            Serilog.Log.Error($"Failde to set port: {exception}");
        }
    }

    void HandleSetCameraEvent(object? sender, SetCameraEventArgs e)
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
            _gameRunner.Start();
        }
        catch (Exception exception)
        {
            Serilog.Log.Error($"Failed to start game: {exception}");
        }
    }

    private void HandleEndGameEvent(object? sender, EventArgs e)
    {
        try
        {
            _gameRunner.End();
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
