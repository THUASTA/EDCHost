using System.Reflection;
using System.Text;
using System.Text.Json;
using EdcHost.ViewerServers.EventArgs;
using EdcHost.ViewerServers.Messages;
using System.Collections.Concurrent;

using Fleck;

using Serilog;

namespace EdcHost.ViewerServers;

/// <summary>
/// ViewerServer handles the API requests from viewers via WebSocket.
/// </summary>
public class ViewerServer : IViewerServer
{
    public ConcurrentQueue<IMessage> PacketsToSend = new();

    readonly ILogger _logger = Log.Logger.ForContext("Component", "ViewerServers");
    public IGameController Controller { get; } = new GameController();
    public event EventHandler<SetPortEventArgs>? SetPortEvent;
    public event EventHandler<SetCameraEventArgs>? SetCameraEvent;

    IWebSocketConnection? _socket = null;
    readonly IWebSocketServer _webSocketServer;

    public Task TaskForSending;


    public ViewerServer(IWebSocketServer webSocketServer, IGameController controller)
    {
        _webSocketServer = webSocketServer;
        Controller = controller;
        TaskForSending = new(() => SendTaskFunc());
    }

    /// <summary>
    /// Starts the server.
    /// </summary>
    public void Start()
    {
        _logger.Information("Starting...");

        WebSocketServerStart();

        Controller.GetHostConfigurationEvent += (sender, args) => Publish(args.Message);

        TaskForSending.Start();

        _logger.Information("Started.");
    }
    /// <summary>
    /// Stops the server.
    /// </summary>
    public void Stop()
    {
        _logger.Information("Stopping...");
        _webSocketServer.Dispose();
        _socket?.Close();
        _logger.Information("Stopped.");
    }

    public void Publish(IMessage message)
    {
        PacketsToSend.Enqueue(message);
    }

    /// <summary>
    /// Sends the message to the viewer.
    /// </summary>
    /// <param name="message">the message to send.</param>
    public void SendTaskFunc()
    {
        _logger.Debug("SendTaskFunc started");

        while (true)
        {
            try
            {
                if (PacketsToSend.TryDequeue(out IMessage? message))
                {
                    byte[] bytes = message.SerializeToUtf8Bytes();
                    if (_socket == null)
                    {
                        throw new Exception("Socket not specified.");
                    }
                    _socket?.Send(bytes);
                }
            }
            catch (Exception e)
            {
                RaiseError((int)ErrorCode.NoSocketConnection, e.Message);
                _logger.Error(e, "Error while sending message.");
            }
        }
        // _logger.Debug("SendTaskFunc ended");
    }

    /// <summary>
    /// Starts the WebSocket server.
    /// </summary>
    void WebSocketServerStart()
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
    void DeserializeMessage(string text)
    {
        try
        {
            IMessage message = JsonSerializer.Deserialize<Message>(text)!;
            switch (message.MessageType)
            {
                case "COMPETITION_CONTROL_COMMAND":
                    _logger.Debug("Received COMPETITION_CONTROL_COMMAND.");
                    ICompetitionControlCommand command
                        = JsonSerializer.Deserialize<CompetitionControlCommand>(text)!;
                    switch (command.Command)
                    {
                        case "START":
                            Controller.StartGame();
                            break;
                        case "END":
                            Controller.EndGame();
                            break;
                        case "RESET":
                            Controller.ResetGame();
                            break;
                        case "GET_HOST_CONFIGURATION":
                            try
                            {
                                Controller.GetHostConfiguration();
                            }
                            catch (Exception e)
                            {
                                RaiseError((int)ErrorCode.NoDeviceAvailable, e.Message);
                                throw new Exception(e.Message);
                            }
                            break;
                        default:
                            RaiseError((int)ErrorCode.InvalidCommand, $"Invalid command: {command.Command}");
                            throw new Exception($"Invalid command: {command.Command}");
                    }
                    break;

                case "HOST_CONFIGURATION_FROM_CLIENT":
                    _logger.Debug("Received HOST_CONFIGURATION_FROM_CLIENT.");
                    IHostConfigurationFromClient hostConfiguration
                        = JsonSerializer.Deserialize<HostConfigurationFromClient>(text)!;
                    Type Player = hostConfiguration.Players.GetType().GetGenericArguments()[0];
                    PropertyInfo[] playerProperties = Player.GetProperties();
                    foreach (object player in hostConfiguration.Players)
                    {
                        int playerId = -1;
                        foreach (PropertyInfo property in playerProperties)
                        {
                            if (property.Name == "id")
                            {
                                playerId = (int)property.GetValue(player)!;
                                if (playerId < 0)
                                {
                                    RaiseError((int)ErrorCode.InvalidPlayer, "Invalid player id.");
                                    throw new Exception("Invalid player id.");
                                }
                            }
                            else if (property.Name == "camera")
                            {
                                object? cameraConfiguration = null;
                                cameraConfiguration = property.GetValue(player);
                                if (cameraConfiguration == null)
                                {
                                    RaiseError((int)ErrorCode.InvalidCamera, "Invalid camera configuration.");
                                    throw new Exception("Invalid camera configuration.");
                                }
                                SetCameraEvent?.Invoke(this, new SetCameraEventArgs(playerId, cameraConfiguration));
                            }
                            else if (property.Name == "serialPort")
                            {
                                Type Port = property.GetValue(player)!.GetType();
                                PropertyInfo[] portProperties = Port.GetProperties();
                                string? portName = null;
                                int baudRate = 0;
                                foreach (PropertyInfo portProperty in portProperties)
                                {
                                    if (portProperty.Name == "portName")
                                    {
                                        portName = (string?)portProperty.GetValue(property.GetValue(player));
                                    }
                                    else if (portProperty.Name == "baudRate")
                                    {
                                        baudRate = (int)portProperty.GetValue(property.GetValue(player))!;
                                    }
                                }
                                if (portName == null || baudRate == 0)
                                {
                                    RaiseError((int)ErrorCode.InvalidPort, "Invalid port configuration.");
                                    throw new Exception("Invalid port configuration.");
                                }
                                SetPortEvent?.Invoke(this, new SetPortEventArgs(playerId, portName, baudRate));
                            }
                        }
                    }
                    break;

                default:
                    RaiseError((int)ErrorCode.InvalidMessageType, $"Invalid message type: {message.MessageType}");
                    throw new Exception($"Invalid message type: {message.MessageType}");
            }
        }
        catch
        {
            RaiseError((int)ErrorCode.InvalidMessage, "Invalid message.");
            _logger.Error("Invalid message.");
        }
    }

    /// <summary>
    /// raise an error message to the viewer.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    public void RaiseError(int errorCode, string errorMessage)
    {
        Publish(new Error(errorCode, errorMessage));
    }
}
