using Lol_Overlay_MVVM.MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class WindowFinderService : IWindowFinderService
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int Left, Top, Right, Bottom; };

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]

        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;

        private IntPtr GetLeagueHWND()
        {
            Process[] processes = Process.GetProcessesByName("Riot Client");

            IntPtr hwnd;

            if (processes.Length > 0)
            {
                hwnd = processes[0].MainWindowHandle;
                return hwnd;
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        public bool isLeagueWindowFocused()
        {
            IntPtr hWnd = GetLeagueHWND();

            if (hWnd == IntPtr.Zero)
            {
                return false;
            }
            return hWnd == GetForegroundWindow();

        }

        public bool isLeagueWindowReady()
        {
            var hWnd = GetLeagueHWND();
            if (hWnd == IntPtr.Zero)
                return false;

            if (!IsWindowVisible(hWnd))
                return false;

            if (GetWindowRect(hWnd, out RECT r))
            {
                int width = r.Right - r.Left;
                int height = r.Bottom - r.Top;
                if (width <= 0 || height <= 0)
                    return false;
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool focusLeagueWindow()
        {
            var hWnd = GetLeagueHWND();

            if (hWnd == IntPtr.Zero)
            {
                return false;
            }

            if (IsIconic(hWnd))
            {
                ShowWindow(hWnd, SW_RESTORE);
            }

            return SetForegroundWindow(hWnd);
        }

        public Rectangle? GetLeagueWindowBounds()
        {
            IntPtr leagueHWND = GetLeagueHWND();

            if (leagueHWND == IntPtr.Zero)
            {
                return null;
            }
            if (GetWindowRect(leagueHWND, out RECT r))
            {
                return Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
            }

            return null;
        }
    }
}
