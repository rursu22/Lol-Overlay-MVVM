using Lol_Overlay_MVVM.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class CalibrationService : ICalibrationService
    {
        public Task<Point> CaptureClickAsync(string instruction)
        {
            // We’ll show the overlay modally and return the click point immediately.
            var overlay = new FullscreenCalibrationOverlay(instruction)
            {
                Owner = Application.Current.MainWindow, // optional
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            // ShowDialog blocks until the window closes.
            var result = overlay.ShowDialog();

            if (overlay.ClickPoint.HasValue)
                return Task.FromResult(overlay.ClickPoint.Value);

            throw new OperationCanceledException("Calibration was canceled.");
        }
    }
}
