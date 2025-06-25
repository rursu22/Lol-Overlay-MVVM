using Lol_Overlay_MVVM.MVVM.Interfaces;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    class ComputerVisionService : IComputerVisionService
    {
        public Mat CaptureScreen()
        {
            
            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            using var bmp = new Bitmap(screen.Width, screen.Height);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            }

            return BitmapConverter.ToMat(bmp);
        }

        public System.Windows.Point? TemplateMatch(Mat rawHaystack, Mat rawNeedle, double threshold = 0.9)
        {
            // In case the haystack image has alpha, keep only the color channels
            Mat haystack = rawHaystack.Channels() switch
            {
                4 => rawHaystack.CvtColor(ColorConversionCodes.BGRA2BGR),
                3 => rawHaystack,
                _ => rawHaystack.CvtColor(ColorConversionCodes.GRAY2BGR)
            };

            Mat needle = rawNeedle;

            if (haystack.Type() != needle.Type())
                throw new InvalidOperationException(
                    $"Haystack type {haystack.Type()} != needle type {needle.Type()}");

            
            int resultCols = haystack.Cols - needle.Cols + 1;
            int resultRows = haystack.Rows - needle.Rows + 1;
            using var result = new Mat(resultRows, resultCols, MatType.CV_32FC1);


            Cv2.MatchTemplate(haystack, needle, result, TemplateMatchModes.CCoeffNormed);

            
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal >= threshold)
            {
                return new System.Windows.Point(
                    maxLoc.X + needle.Cols / 2,
                    maxLoc.Y + needle.Rows / 2);
            }
            return null;
        }

    }
}
