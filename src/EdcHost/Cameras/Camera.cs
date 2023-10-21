using System.Drawing;
using OpenCvSharp;

namespace EdcHost.Cameras;

class Camera : ICamera
{
    public Image? Image { get; } = null;
    public IPosition<int>? TargetPosition { get; } = null;
    readonly VideoCapture _videoCapture;

    public Camera(VideoCapture videoCapture)
    {
        _videoCapture = videoCapture;
    }

    ~Camera()
    {
        _videoCapture.Dispose();
    }
}
