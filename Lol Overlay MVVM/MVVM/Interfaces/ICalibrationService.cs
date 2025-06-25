using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public interface ICalibrationService
    {
        public Task<System.Windows.Point> CaptureClickAsync(string instruction);
    }
}
