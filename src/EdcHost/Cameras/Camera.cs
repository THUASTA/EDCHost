using System.Drawing;
using OpenCvSharp;

namespace EdcHost.Cameras;

public class Camera : ICamera
{
    public Image? Image { get; } = null;
    public IPosition<int>? TargetPosition { get; } = null;
    private readonly VideoCapture _videoCapture;

    public Camera(VideoCapture videoCapture)
    {
        _videoCapture = videoCapture;
    }

    ~Camera()
    {
        _videoCapture.Dispose();
    }
}
