using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class LoginService : ILoginService
    {
        const uint INPUT_MOUSE = 0;
        const uint INPUT_KEYBOARD = 1;

        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint KEYEVENTF_UNICODE = 0x0004;

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

        private static void SendText(string text)
        {
            var inputs = new INPUT[text.Length * 2];
            int idx = 0;

            foreach (char c in text)
            {
                // Handle the key down event
                inputs[idx].type = INPUT_KEYBOARD;
                inputs[idx].U.ki.wVk = 0;
                inputs[idx].U.ki.wScan = c;
                inputs[idx].U.ki.dwFlags = KEYEVENTF_UNICODE;
                idx++;

                // Handle the key up event
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

        public void clickLogin()
        {
            Click(loginButtonPosition);
        }
    }
}
