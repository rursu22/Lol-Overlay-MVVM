using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Lol_Overlay_MVVM.MVVM.Interfaces
{
    public interface IComputerVisionService
    {
        Mat CaptureScreen();

        System.Windows.Point? TemplateMatch(Mat haystack, Mat needle, double threshold = 0.9);
    }
}
