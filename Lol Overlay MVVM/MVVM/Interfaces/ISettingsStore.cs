using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Interfaces
{
    public interface ISettingsStore
    {
        public Task<string?> RetrieveTheme();

        public Task ModifyTheme(string newTheme);
    }
}
