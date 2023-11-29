using EdcHost.ViewerServers;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandleAfterMessageReceiveEvent(object? sender, ViewerServers.AfterMessageReceiveEventArgs e)
    {
        switch (e.Message)
        {
            case ViewerServers.CompetitionControlCommandMessage message:
                switch (message.Command)
                {
                    case "START":
                        HandleStartGame();
                        break;

                    case "END":
                        HandleEndGame();
                        break;

                    case "RESET":
                        HandleResetGame();
                        break;

                    case "GET_HOST_CONFIGURATION":
                        HandleGetHostConfiguration();
                        break;
                    default:
                        _logger.Error($"Invalid command: {message.Command}.");
                        break;
                }
                break;

            case ViewerServers.HostConfigurationFromClientMessage message:
                HandleUpdateConfiguration(message);
                break;

            default:
                _logger.Error($"Invalid message type: {e.Message.MessageType}.");
                break;
        }
    }

    void HandleStartGame()
    {
        _gameRunner.Start();
    }

    void HandleEndGame()
    {
        _gameRunner.End();
    }

    void HandleResetGame()
    {
        _logger.Information("Resetting Game...");
        if (_gameRunner.IsRunning)
        {
            _logger.Information("Game is running, Stopping.");
            _gameRunner.End();
        }

        _game = Games.IGame.Create(
            diamondMines: _config.Game.DiamondMines,
            goldMines: _config.Game.GoldMines,
            ironMines: _config.Game.IronMines
        );
        _gameRunner = Games.IGameRunner.Create(_game);

        _game.AfterJudgementEvent += HandleAfterJudgementEvent;

        for (int i = 0; i < _game.Players.Count; i++)
        {
            _game.Players[i].OnAttack += HandlePlayerAttackEvent;
            _game.Players[i].OnPlace += HandlePlayerPlaceEvent;
            _game.Players[i].OnDig += HandlePlayerDigEvent;
        }
        _logger.Information("Done.");
    }

    void HandleGetHostConfiguration()
    {
        ViewerServers.HostConfigurationFromServerMessage configMessage = new ViewerServers.HostConfigurationFromServerMessage()
        {
            Players = _game.Players.Select((player, playerIndex) =>
                {
                    if (_playerHardwareInfo.Count <= playerIndex)
                    {
                        // Empty
                        return new ViewerServers.HostConfigurationFromServerMessage.PlayerType();
                    }

                    int? cameraIndex = _playerHardwareInfo[playerIndex].CameraIndex;

                    CameraServers.ICamera? camera = null;
                    if (cameraIndex is not null)
                    {
                        camera = _cameraServer.GetCamera(cameraIndex.Value);
                    }

                    string? portName = _playerHardwareInfo[playerIndex].PortName;
                    int baudRate = _playerHardwareInfo[playerIndex].BaudRate;

                    return new ViewerServers.HostConfigurationFromServerMessage.PlayerType()
                    {
                        Camera = cameraIndex == null ? new() : new()
                        {
                            CameraId = cameraIndex!.Value,

                            Recognition = new ViewerServers.HostConfigurationFromServerMessage.PlayerType.CameraType.RecognitionType()
                            {
                                HueCenter = camera!.Locator.Options.HueCenter,
                                HueRange = camera!.Locator.Options.HueRange,
                                SaturationCenter = camera!.Locator.Options.SaturationCenter,
                                SaturationRange = camera!.Locator.Options.SaturationRange,
                                ValueCenter = camera!.Locator.Options.ValueCenter,
                                ValueRange = camera!.Locator.Options.ValueRange,
                                MinArea = camera!.Locator.Options.MinArea,
                                ShowMask = camera!.Locator.Options.ShowMask
                            },

                            Calibration = camera!.Locator.Options.Calibrate == false ? new() : new()
                            {
                                TopLeft = new ViewerServers.HostConfigurationFromServerMessage.PlayerType.CameraType.CalibrationType.Point()
                                {
                                    X = camera!.Locator.Options.TopLeftX,
                                    Y = camera!.Locator.Options.TopLeftY
                                },
                                TopRight = new ViewerServers.HostConfigurationFromServerMessage.PlayerType.CameraType.CalibrationType.Point()
                                {
                                    X = camera!.Locator.Options.TopRightX,
                                    Y = camera!.Locator.Options.TopRightY
                                },
                                BottomLeft = new ViewerServers.HostConfigurationFromServerMessage.PlayerType.CameraType.CalibrationType.Point()
                                {
                                    X = camera!.Locator.Options.BottomLeftX,
                                    Y = camera!.Locator.Options.BottomLeftY
                                },
                                BottomRight = new ViewerServers.HostConfigurationFromServerMessage.PlayerType.CameraType.CalibrationType.Point()
                                {
                                    X = camera!.Locator.Options.BottomRightX,
                                    Y = camera!.Locator.Options.BottomRightY
                                }
                            }
                        },
                        SerialPort = portName == null ? new() : new()
                        {
                            PortName = portName,
                            BaudRate = baudRate,
                        }
                    };
                }
            ).ToList(),
            AvailableCameras = _cameraServer.AvailableCameraIndexes,
            AvailableSerialPorts = _slaveServer.AvailablePortNames
        };

        _viewerServer.Publish(configMessage);
    }


    void HandleUpdateConfiguration(ViewerServers.HostConfigurationFromClientMessage message)
    {
        try
        {
            foreach (ViewerServers.HostConfigurationFromClientMessage.PlayerType player in message.Players)
            {
                // Do not need to check if a player exists because we do not care.

                PlayerHardwareInfo playerHardwareInfo = new();

                if (player.Camera is not null)
                {
                    playerHardwareInfo.CameraIndex = player.Camera.CameraId;

                    if (_cameraServer.GetCamera(player.Camera.CameraId) != null)
                    {
                        _logger.Error($"Camera {player.Camera.CameraId} is already opened.");
                        _viewerServer.Publish(new ViewerServers.ErrorMessage()
                        {
                            Message = $"Camera {player.Camera.CameraId} is already opened."
                        });
                        continue;
                    }

                    CameraServers.ICamera camera = _cameraServer.OpenCamera(player.Camera.CameraId, new CameraServers.Locator());

                    _logger.Information($"Camera {player.Camera.CameraId} opened.");

                    CameraServers.RecognitionOptions recognitionOptions = new()
                    {
                        HueCenter = player.Camera.Recognition.HueCenter,
                        HueRange = player.Camera.Recognition.HueRange,
                        SaturationCenter = player.Camera.Recognition.SaturationCenter,
                        SaturationRange = player.Camera.Recognition.SaturationRange,
                        ValueCenter = player.Camera.Recognition.ValueCenter,
                        ValueRange = player.Camera.Recognition.ValueRange,
                        MinArea = player.Camera.Recognition.MinArea,
                        ShowMask = player.Camera.Recognition.ShowMask
                    };

                    if (player.Camera.Calibration is not null)
                    {
                        recognitionOptions.Calibrate = true;

                        recognitionOptions.TopLeftX = player.Camera.Calibration.TopLeft.X;
                        recognitionOptions.TopLeftY = player.Camera.Calibration.TopLeft.Y;
                        recognitionOptions.TopRightX = player.Camera.Calibration.TopRight.X;
                        recognitionOptions.TopRightY = player.Camera.Calibration.TopRight.Y;
                        recognitionOptions.BottomLeftX = player.Camera.Calibration.BottomLeft.X;
                        recognitionOptions.BottomLeftY = player.Camera.Calibration.BottomLeft.Y;
                        recognitionOptions.BottomRightX = player.Camera.Calibration.BottomRight.X;
                        recognitionOptions.BottomRightY = player.Camera.Calibration.BottomRight.Y;
                    }

                    camera.Locator = new CameraServers.Locator(recognitionOptions);
                }

                if (player.SerialPort is not null)
                {
                    playerHardwareInfo.PortName = player.SerialPort.PortName;
                    playerHardwareInfo.BaudRate = player.SerialPort.BaudRate;

                    _slaveServer.OpenPort(
                        portName: player.SerialPort.PortName,
                        baudRate: player.SerialPort.BaudRate
                    );
                }

                _playerHardwareInfo.AddOrUpdate(player.PlayerId, playerHardwareInfo, (_, _) => playerHardwareInfo);
            }
        }
        catch (Exception e)
        {
            _logger.Error("Error updating configuration: {0}", e.Message);
            _viewerServer.Publish(new ViewerServers.ErrorMessage());
        }
    }

}
