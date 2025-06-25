using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Interfaces
{
    public interface IWindowFinderService
    {
        public bool isLeagueWindowFocused();

        public bool isLeagueWindowReady();

        public bool focusLeagueWindow();

        public Rectangle? GetLeagueWindowBounds();
    }
}
