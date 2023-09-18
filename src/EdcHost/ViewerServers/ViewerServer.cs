using System.Net.Mime;
using System.Text;
using System.Text.Json;

using EdcHost.ViewerServers.Messages;

using Fleck;

using Serilog;

namespace EdcHost.ViewerServers;

/// <summary>
/// ViewerServer handles the API requests from viewers via WebSocket.
/// </summary>
public class ViewerServer
{
    private readonly WebSocketServer _webSocketServer;
    private readonly ILogger _logger = Log.Logger.ForContext<ViewerServer>();
    private IWebSocketConnection? _socket = null;
    public Updater CompetitionUpdater { get; } = new Updater();

    public ViewerServer(int port)
    {
        _webSocketServer = new WebSocketServer("ws://localhost:" + port)
        {
            RestartAfterListenError = true
        };
    }

    /// <summary>
    /// Starts the server.
    /// </summary>
    public void Start()
    {
        WebSocketServerStart();
        CompetitionUpdater.StartUpdate();
        _logger.Information("Server started.");
    }
    /// <summary>
    /// Stops the server.
    /// </summary>
    public void Stop()
    {
        _webSocketServer.Dispose();
        _socket?.Close();
        CompetitionUpdater.End();
        _logger.Information("Server stopped.");
    }

    /// <summary>
    /// Sends the message to the viewer.
    /// </summary>
    /// <param name="message">the message to send.</param>
    public void Send(IMessage message)
    {
        try
        {
            byte[] bytes = message.SerializeToUtf8Bytes();
            if (_socket == null)
            {
                throw new Exception("Socket not specified.");
            }
            _socket?.Send(bytes);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error while sending message.");
        }
    }

    /// <summary>
    /// Starts the WebSocket server.
    /// </summary>
    private void WebSocketServerStart()
    {
        _webSocketServer.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                _logger.Debug("WebSocket connection opened.");
                if (_socket == null)
                {
                    _socket = socket;
                }
            };

            socket.OnClose = () =>
            {
                _logger.Debug("WebSocket connection closed.");
            };

            socket.OnMessage = text =>
            {
                try
                {
                    DeserializeMessage(text);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error while parsing message.");
                    socket.Close();
                }
            };

            socket.OnBinary = bytes =>
            {
                try
                {
                    string text = Encoding.UTF8.GetString(bytes);
                    DeserializeMessage(text);
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error while parsing message.");
                    socket.Close();
                }
            };

            socket.OnError = exception =>
            {
                _logger.Error(exception, "Error while receiving message.");
                socket.Close();
            };
        });
    }

    /// <summary>
    /// Deserializes the message and calls the appropriate method.
    /// </summary>
    /// <param name="text"></param>
    /// <exception cref="InvalidDataException"></exception>
    private void DeserializeMessage(string text)
    {
        IMessage message = JsonSerializer.Deserialize<Message>(text)!;
        switch (message.MessageType)
        {
            case "COMPETITION_CONTROL_COMMAND":
                ICompetitionControlCommand command
                    = JsonSerializer.Deserialize<CompetitionControlCommand>(text)!;
                //TODO: change the game stage.
                break;

            case "HOST_CONFIGURATION_FROM_CLIENT":
                IHostConfigurationFromClient hostConfiguration
                    = JsonSerializer.Deserialize<HostConfigurationFromClient>(text)!;
                //TODO: change the configuration
                break;

            default:
                throw new InvalidDataException("Invalide message type.");
        }
    }
}
