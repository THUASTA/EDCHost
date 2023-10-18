using System.Drawing;
using OpenCvSharp;

namespace EdcHost.Cameras;

public interface ICamera
{
    public static ICamera Create(int cameraIndex)
    {
        VideoCapture videoCapture = new(cameraIndex);
        return new Camera(videoCapture);
    }

    public Image? Image { get; }
    public IPosition<int>? TargetPosition { get; }
}
