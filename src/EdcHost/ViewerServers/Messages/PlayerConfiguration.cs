using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public struct PlayerConfiguration
{
    public struct CameraInfo
    {
        public struct QuadCalibration
        {
            public struct Point2d
            {
                [JsonPropertyName("x")]
                public int X { get; private set; }
                [JsonPropertyName("y")]
                public int Y { get; private set; }
            }
            [JsonPropertyName("topLeft")]
            public Point2d TopLeft { get; private set; }
            [JsonPropertyName("topRight")]
            public Point2d TopRight { get; private set; }
            [JsonPropertyName("bottomLeft")]
            public Point2d BottomLeft { get; private set; }
            [JsonPropertyName("bottomRight")]
            public Point2d BottomRight { get; private set; }
        }

        public struct RecognitionParams
        {
            [JsonPropertyName("hueCenter")]
            public int HueCenter { get; private set; }
            [JsonPropertyName("hueRange")]
            public int HueRange { get; private set; }
            [JsonPropertyName("saturationCenter")]
            public int SaturationCenter { get; private set; }
            [JsonPropertyName("saturationRange")]
            public int SaturationRange { get; private set; }
            [JsonPropertyName("valueCenter")]
            public int ValueCenter { get; private set; }
            [JsonPropertyName("valueRange")]
            public int ValueRange { get; private set; }
            [JsonPropertyName("minArea")]
            public int MinArea { get; private set; }
            [JsonPropertyName("showMask")]
            public bool ShowMask { get; private set; }
        }

        [JsonPropertyName("cameraId")]
        public int CameraId { get; private set; }
        [JsonPropertyName("calibration")]
        public QuadCalibration calibration { get; private set; }
        [JsonPropertyName("recognition")]
        public RecognitionParams Recognition { get; private set; }
    }

    public struct SerialPortInfo
    {
        [JsonPropertyName("portName")]
        public string PortName { get; private set; }
        [JsonPropertyName("baudRate")]
        public int BaudRate { get; private set; }
    }

    [JsonPropertyName("playerId")]
    public int PlayerId { get; private set; }

    [JsonPropertyName("camera")]
    public CameraInfo Camera { get; private set; }

    [JsonPropertyName("serialPort")]
    public SerialPortInfo SerialPort { get; private set; }
}
