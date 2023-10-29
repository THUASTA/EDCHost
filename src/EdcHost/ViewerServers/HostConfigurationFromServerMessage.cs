using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers;

public record HostConfigurationFromServerMessage : Message
{

    public record PlayerType
    {
        public record CameraType
        {
            public record CalibrationType
            {
                public record Point
                {
                    [JsonPropertyName("x")]
                    public float X { get; init; } = 0.0f;

                    [JsonPropertyName("y")]
                    public float Y { get; init; } = 0.0f;
                }

                [JsonPropertyName("topLeft")]
                public Point TopLeft { get; init; } = new();

                [JsonPropertyName("topRight")]
                public Point TopRight { get; init; } = new();

                [JsonPropertyName("bottomLeft")]
                public Point BottomLeft { get; init; } = new();

                [JsonPropertyName("bottomRight")]
                public Point BottomRight { get; init; } = new();
            }

            public record RecognitionType
            {
                [JsonPropertyName("hueCenter")]
                public float HueCenter { get; init; } = 0;

                [JsonPropertyName("hueRange")]
                public float HueRange { get; init; } = 0;

                [JsonPropertyName("saturationCenter")]
                public float SaturationCenter { get; init; } = 0;

                [JsonPropertyName("saturationRange")]
                public float SaturationRange { get; init; } = 0;

                [JsonPropertyName("valueCenter")]
                public float ValueCenter { get; init; } = 0;

                [JsonPropertyName("valueRange")]
                public float ValueRange { get; init; } = 0;

                [JsonPropertyName("minArea")]
                public float MinArea { get; init; } = 0;

                [JsonPropertyName("maxArea")]
                public float MaxArea { get; init; } = 0;

                [JsonPropertyName("showMask")]
                public bool ShowMask { get; init; } = false;
            }

            [JsonPropertyName("cameraId")]
            public int CameraId { get; init; } = 0;

            [JsonPropertyName("calibration")]
            public CalibrationType Calibration { get; init; } = new();

            [JsonPropertyName("recognition")]
            public RecognitionType Recognition { get; init; } = new();
        }

        public record SerialPortType
        {
            [JsonPropertyName("portName")]
            public string PortName { get; init; } = "";

            [JsonPropertyName("baudRate")]
            public int BaudRate { get; init; } = 0;
        }

        [JsonPropertyName("playerId")]
        public int PlayerId { get; init; } = 0;

        [JsonPropertyName("camera")]
        public CameraType Camera { get; init; } = new();

        [JsonPropertyName("serialPort")]
        public SerialPortType SerialPort { get; init; } = new();
    }

    [JsonPropertyName("messageType")]
    public override string MessageType { get; init; } = "HOST_CONFIGURATION_FROM_SERVER";

    [JsonPropertyName("availableCameras")]
    public List<int> AvailableCameras { get; init; } = new();

    [JsonPropertyName("availableSerialPorts")]
    public List<string> AvailableSerialPorts { get; init; } = new();


    [JsonPropertyName("players")]
    public List<PlayerType> Players { get; init; } = new();
}
