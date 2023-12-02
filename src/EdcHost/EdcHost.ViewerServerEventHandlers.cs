using System.Diagnostics;

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
        ViewerServers.HostConfigurationFromServerMessage configMessage = new()
        {
            AvailableCameras = _cameraServer.AvailableCameraIndexes,
            AvailableSerialPorts = _slaveServer.AvailablePortNames,
            Configuration = new()
            {
                Cameras = _cameraServer.OpenCameraIndexes.Select((cameraIndex) =>
                    {
                        CameraServers.ICamera? cameraOrNull = _cameraServer.GetCamera(cameraIndex);
                        Debug.Assert(cameraOrNull != null);

                        CameraServers.ICamera camera = cameraOrNull!;

                        return new ViewerServers.HostConfiguration.CameraType()
                        {
                            CameraId = cameraIndex,

                            Recognition = new ViewerServers.HostConfiguration.CameraType.RecognitionType()
                            {
                                HueCenter = camera.Locator.Options.HueCenter,
                                HueRange = camera.Locator.Options.HueRange,
                                SaturationCenter = camera.Locator.Options.SaturationCenter,
                                SaturationRange = camera.Locator.Options.SaturationRange,
                                ValueCenter = camera.Locator.Options.ValueCenter,
                                ValueRange = camera.Locator.Options.ValueRange,
                                MinArea = camera.Locator.Options.MinArea,
                                ShowMask = camera.Locator.Options.ShowMask
                            },

                            Calibration = camera.Locator.Options.Calibrate == false ? new() : new()
                            {
                                TopLeft = new ViewerServers.HostConfiguration.CameraType.CalibrationType.Point()
                                {
                                    X = camera.Locator.Options.TopLeftX,
                                    Y = camera.Locator.Options.TopLeftY
                                },
                                TopRight = new ViewerServers.HostConfiguration.CameraType.CalibrationType.Point()
                                {
                                    X = camera.Locator.Options.TopRightX,
                                    Y = camera.Locator.Options.TopRightY
                                },
                                BottomLeft = new ViewerServers.HostConfiguration.CameraType.CalibrationType.Point()
                                {
                                    X = camera.Locator.Options.BottomLeftX,
                                    Y = camera.Locator.Options.BottomLeftY
                                },
                                BottomRight = new ViewerServers.HostConfiguration.CameraType.CalibrationType.Point()
                                {
                                    X = camera.Locator.Options.BottomRightX,
                                    Y = camera.Locator.Options.BottomRightY
                                }
                            }
                        };
                    }).ToList(),
                Players = _playerHardwareInfo.Select((kv) =>
                    {
                        int playerIndex = kv.Key;
                        PlayerHardwareInfo playerHardwareInfo = kv.Value;

                        return new ViewerServers.HostConfiguration.PlayerType()
                        {
                            PlayerId = playerIndex,
                            Camera = playerHardwareInfo.CameraIndex,
                            SerialPort = playerHardwareInfo.PortName
                        };
                    }).ToList(),
                SerialPorts = _slaveServer.OpenPortNames.Select((portName) =>
                    {
                        SlaveServers.ISlaveServer.PortInfo? portInfoOrNull = _slaveServer.GetPortInfo(portName);
                        Debug.Assert(portInfoOrNull != null);
                        SlaveServers.ISlaveServer.PortInfo portInfo = portInfoOrNull!;

                        return new ViewerServers.HostConfiguration.SerialPortType()
                        {
                            PortName = portName,
                            BaudRate = portInfo.BaudRate
                        };
                    }).ToList(),
            },
        };

        _viewerServer.Publish(configMessage);
    }


    void HandleUpdateConfiguration(ViewerServers.HostConfigurationFromClientMessage message)
    {
        try
        {
            foreach (ViewerServers.HostConfiguration.CameraType camera in message.Configuration.Cameras)
            {
                if (!_cameraServer.OpenCameraIndexes.Contains(camera.CameraId))
                {
                    _logger.Information($"Opening camera {camera.CameraId}...");
                    _cameraServer.OpenCamera(camera.CameraId, new CameraServers.Locator());
                }

                CameraServers.ICamera? cameraOrNull = _cameraServer.GetCamera(camera.CameraId);
                Debug.Assert(cameraOrNull != null);
                CameraServers.ICamera cameraInstance = cameraOrNull!;

                CameraServers.RecognitionOptions recognitionOptions = cameraInstance.Locator.Options;

                if (camera.Recognition is not null)
                {
                    recognitionOptions.HueCenter = camera.Recognition.HueCenter;
                    recognitionOptions.HueRange = camera.Recognition.HueRange;
                    recognitionOptions.SaturationCenter = camera.Recognition.SaturationCenter;
                    recognitionOptions.SaturationRange = camera.Recognition.SaturationRange;
                    recognitionOptions.ValueCenter = camera.Recognition.ValueCenter;
                    recognitionOptions.ValueRange = camera.Recognition.ValueRange;
                    recognitionOptions.MinArea = camera.Recognition.MinArea;
                    recognitionOptions.ShowMask = camera.Recognition.ShowMask;
                }

                if (camera.Calibration is not null)
                {
                    recognitionOptions.Calibrate = true;

                    recognitionOptions.TopLeftX = camera.Calibration.TopLeft.X;
                    recognitionOptions.TopLeftY = camera.Calibration.TopLeft.Y;
                    recognitionOptions.TopRightX = camera.Calibration.TopRight.X;
                    recognitionOptions.TopRightY = camera.Calibration.TopRight.Y;
                    recognitionOptions.BottomLeftX = camera.Calibration.BottomLeft.X;
                    recognitionOptions.BottomLeftY = camera.Calibration.BottomLeft.Y;
                    recognitionOptions.BottomRightX = camera.Calibration.BottomRight.X;
                    recognitionOptions.BottomRightY = camera.Calibration.BottomRight.Y;
                }

                // TODO: Update camera properties
            }

            foreach (ViewerServers.HostConfiguration.SerialPortType serialPort in message.Configuration.SerialPorts)
            {
                if (!_slaveServer.OpenPortNames.Contains(serialPort.PortName))
                {
                    _logger.Information($"Opening serial port {serialPort.PortName}...");
                    if (serialPort.BaudRate is not null)
                    {
                        _slaveServer.OpenPort(serialPort.PortName, serialPort.BaudRate.Value);
                    }
                    else
                    {
                        _slaveServer.OpenPort(serialPort.PortName);
                    }
                }
            }

            foreach (ViewerServers.HostConfiguration.PlayerType player in message.Configuration.Players)
            {
                // Do not need to check if a player exists because we do not care.

                PlayerHardwareInfo playerHardwareInfo = new()
                {
                    CameraIndex = player.Camera,
                    PortName = player.SerialPort
                };

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
