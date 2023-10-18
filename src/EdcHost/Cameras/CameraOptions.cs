namespace EdcHost.Cameras;

public class CameraOptions
{
    public int MinHue { get; }
    public int MinSaturation { get; }
    public int MinValue { get; }
    public int MaxHue { get; }
    public int MaxSaturation { get; }
    public int MaxValue { get; }
    public int MinArea { get; }

    public CameraOptions(int minHue = 0, int minSaturation = 0, int minValue = 0, int maxHue = 255, int maxSaturation = 255, int maxValue = 255, int minArea = 0)
    {
        MinHue = minHue;
        MinSaturation = minSaturation;
        MinValue = minValue;
        MaxHue = maxHue;
        MaxSaturation = maxSaturation;
        MaxValue = maxValue;
        MinArea = minArea;
    }
}
