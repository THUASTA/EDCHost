using EdcHost.ViewerServers.EventArgs;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandleSetPortEvent(object? sender, SetPortEventArgs e)
    {
        _logger.Information("[Update]");
        _logger.Information($"Player {e.PlayerId}:");
        _logger.Information($"Port: {e.PortName} BaudRate: {e.BaudRate}");
    }

    void HandleSetCameraEvent(object? sender, SetCameraEventArgs e)
    {
        _logger.Information("[Update]");
        _logger.Information($"Player {e.PlayerId}:");
        _logger.Information($"Camera: {e.CameraConfiguration}");
    }
}
