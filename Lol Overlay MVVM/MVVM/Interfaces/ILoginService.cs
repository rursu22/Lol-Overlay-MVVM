using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public interface ILoginService
    {
        public void setUsernameBoxPosition(System.Windows.Point position);

        public void setPasswordBoxPosition(System.Windows.Point position);

        public void setLoginButtonPosition(System.Windows.Point position);

        public System.Windows.Point getUsernameBoxPosition();

        public System.Windows.Point getPasswordBoxPosition();

        public System.Windows.Point getLoginButtonPosition();

        public void clickUsernameAndType(string text, int postClickDelayMS = 100);

        public void clickPasswordAndType(string text, int postClickDelayMS = 100);

        public void clickLogin();

        public Task ReloginAsync(string username, string password);
    }
}
