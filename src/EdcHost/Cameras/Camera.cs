using OpenCvSharp;

namespace EdcHost.Cameras;

class Camera : ICamera
{
    public Mat? Frame { get; private set; } = null;
    public IPosition<float>? TargetPosition { get; private set; } = null;
    readonly VideoCapture _videoCapture;
    readonly ICamera.CameraOptions _cameraOptions;

    public Camera(VideoCapture videoCapture)
    {
        _videoCapture = videoCapture;
        _cameraOptions = new ICamera.CameraOptions();
    }
    public void ProcessImage()
    {
        Mat Oriframe = new Mat();
        _videoCapture.Read(Oriframe);
        if (!Oriframe.Empty())
        {
            Frame = Oriframe.Clone();
            // Convert the color space to HSV
            Cv2.CvtColor(Frame, Frame, ColorConversionCodes.RGB2HSV);
            // Binarization
            Mat mask = new Mat();
            Cv2.InRange(
                src: Frame,
                lowerb: new Scalar(
                    _cameraOptions.MinHue,
                    _cameraOptions.MinSaturation,
                    _cameraOptions.MinValue
                ),
                upperb: new Scalar(
                    _cameraOptions.MaxHue,
                    _cameraOptions.MaxSaturation,
                    _cameraOptions.MaxValue
                ),
                dst: Frame
                );
            OpenCvSharp.Point[][] contourList = Cv2.FindContoursAsArray(mask, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            bool isTargetFound = false;

            if (contourList.Length > 0)
            {
                int maxLength = 0;
                int maxLengthIndex = 0;
                for (int i = 0; i < contourList.Length; i++)
                {
                    int currentLength = contourList[i].Length;
                    if (currentLength > maxLength)
                    {
                        maxLength = currentLength;
                        maxLengthIndex = i;
                    }
                }
                Moments moments = Cv2.Moments(contourList[maxLengthIndex]);

                if ((decimal)moments.M00 >= _cameraOptions.MinArea)
                {
                    TargetPosition = new Position<float>((float)(moments.M10 / moments.M00), (float)(moments.M01 / moments.M00));
                    isTargetFound = true;
                }
            }

            if (!isTargetFound)
            {
                TargetPosition = null;
            }
        }
    }
    ~Camera()
    {
        _videoCapture.Dispose();
    }

}
