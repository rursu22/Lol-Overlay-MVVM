using Lol_Overlay_MVVM.MVVM.Interfaces;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class LoginService : ILoginService
    {
        private readonly IComputerVisionService _cvService;
        private readonly string _loggedInTemplatePath = "Images/loggedIn.png";
        private readonly string _logOutTemplatePath = "Images/logoutButton.png";
        private readonly string _loginTemplate = "Images/login.png";
        private readonly string _alreadyLoggedInTemplate = "Images/alreadyLoggedInTemplate.png";
        private readonly string _playTemplate = "Images/playTemplate.png";

        const uint INPUT_MOUSE = 0;
        const uint INPUT_KEYBOARD = 1;

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_UNICODE = 0x0004;

        private const ushort VK_CONTROL = 0x11;
        private const ushort VK_A = 0x41;
        private const ushort VK_DELETE = 0x2E;

        private System.Windows.Point usernameBoxPosition = new System.Windows.Point(380, 380);
        private System.Windows.Point passwordBoxPosition = new System.Windows.Point(380, 440);
        private System.Windows.Point loginButtonPosition = new System.Windows.Point(380, 780);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx, dy;
            public uint mouseData, dwFlags, time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        public LoginService(IComputerVisionService cvService)
        {
            _cvService = cvService; 
        }

        public void setUsernameBoxPosition(System.Windows.Point position)
        {
            usernameBoxPosition = position;
        }

        public void setPasswordBoxPosition(System.Windows.Point position)
        {
            passwordBoxPosition = position;
        }

        public void setLoginButtonPosition(System.Windows.Point position)
        {
            loginButtonPosition = position;
        }

        public System.Windows.Point getUsernameBoxPosition()
        {
            return usernameBoxPosition;
        }

        public System.Windows.Point getPasswordBoxPosition()
        {
            return passwordBoxPosition;
        }

        public System.Windows.Point getLoginButtonPosition()
        {
            return loginButtonPosition;
        }

        public void Click(System.Windows.Point position)
        {

            SetCursorPos((int)position.X, (int)position.Y);
            var inputs = new INPUT[2];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].U.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].U.mi.dwFlags = MOUSEEVENTF_LEFTUP;

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        }


        private void SendKey(ushort vk, bool keyUp = false)
        {
            var input = new INPUT
            {
                type = INPUT_KEYBOARD,
                U = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = vk,
                        wScan = 0,
                        dwFlags = keyUp ? KEYEVENTF_KEYUP : 0,
                        time = 0,
                        dwExtraInfo = UIntPtr.Zero
                    }
                }
            };
            SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
        }

        private void ClearTextBox()
        {
            // Function to start CTRL-A + Delete keyboard events, i.e, select everything and then delete.
            SendKey(VK_CONTROL, keyUp: false);
            SendKey(VK_A, keyUp: false);
            SendKey(VK_A, keyUp: true);
            SendKey(VK_CONTROL, keyUp: true);

            Thread.Sleep(50);

            SendKey(VK_DELETE, keyUp: false);
            SendKey(VK_DELETE, keyUp: true);
        }

        private async Task CheckForPlayButton()
        {
            var playTemplate = Cv2.ImRead(_playTemplate, ImreadModes.Color);
            var alreadyLoggedInTemplate = Cv2.ImRead(_alreadyLoggedInTemplate, ImreadModes.Color);
            for( int i = 0; i < 7; i++)
            {
                await Task.Delay(1000);
                var screen = _cvService.CaptureScreen();
                var playTemplateMatched = _cvService.TemplateMatch(screen, playTemplate);
                var alreadyLoggedIn = _cvService.TemplateMatch(screen, alreadyLoggedInTemplate);

                if(alreadyLoggedIn.HasValue)
                {
                    break;
                }

                if (playTemplateMatched.HasValue)
                {
                    Click(playTemplateMatched.Value);
                    break;
                }
            }
            
            
        }

        public async Task ReloginAsync(string username, string password)
        {
            var screen = _cvService.CaptureScreen();
            var loggedInTemplate = Cv2.ImRead(_loggedInTemplatePath, ImreadModes.Color);
            var profileClick = _cvService.TemplateMatch(screen, loggedInTemplate);
            System.Windows.Point? loginUI = null;

            // Logout logic
            if (profileClick.HasValue)
            {

                Click(profileClick.Value);
                await Task.Delay(300); 
                var logoutTemplate = Cv2.ImRead(_logOutTemplatePath, ImreadModes.Color);
                screen = _cvService.CaptureScreen();
                var logoutClick = _cvService.TemplateMatch(screen, logoutTemplate);
                if (!logoutClick.HasValue)
                {
                    Debug.WriteLine("Cannot find logout button");
                } else
                {
                    Click(logoutClick.Value);
                }
            }

            // Login logic
            var loginTemplate = Cv2.ImRead(_loginTemplate, ImreadModes.Color);
            loginUI = null;
            for (int i = 0; i < 15; i++)
            {
                await Task.Delay(1000);
                screen = _cvService.CaptureScreen();
                loginUI = _cvService.TemplateMatch(screen, loginTemplate);

                if (loginUI.HasValue)
                {
                    break;
                }
            }
            if (!loginUI.HasValue)
            {
                Debug.WriteLine("Could not find login screen");
            } else
            {
                clickUsernameAndType(username);
                clickPasswordAndType(password);
                clickLogin();
            }
        }

        

        private static void SendText(string text)
        {
            var inputs = new INPUT[text.Length * 2];
            int idx = 0;

            foreach (char c in text)
            {
                inputs[idx].type = INPUT_KEYBOARD;
                inputs[idx].U.ki.wVk = 0;
                inputs[idx].U.ki.wScan = c;
                inputs[idx].U.ki.dwFlags = KEYEVENTF_UNICODE;
                idx++;

                inputs[idx].type = INPUT_KEYBOARD;
                inputs[idx].U.ki.wVk = 0;
                inputs[idx].U.ki.wScan = c;
                inputs[idx].U.ki.dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP;
                idx++;
            }

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        }
        private void ClickAndType(System.Windows.Point position, string text, int postClickDelayMs)
        {
            Click(position);
            Thread.Sleep(postClickDelayMs);
            ClearTextBox();
            Thread.Sleep(50);
            SendText(text);
        }

        public void clickUsernameAndType(string text, int postClickDelayMs = 100)
        {
            ClickAndType(usernameBoxPosition, text, postClickDelayMs);
        }

        public void clickPasswordAndType(string text, int postClickDelayMs = 100)
        {
            ClickAndType(passwordBoxPosition, text, postClickDelayMs);
        }

        public async void clickLogin()
        {
            Click(loginButtonPosition);
            await CheckForPlayButton();
        }
    }
}
