using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowHerder.Window
{
    public class NativeWindowStateManager : IWindowStateManager
    {
        #region Win32 API

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern IntPtr BeginDeferWindowPos(int nNumWindows);

        [DllImport("user32.dll")]
        static extern IntPtr DeferWindowPos(IntPtr hWinPosInfo, IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool EndDeferWindowPos(IntPtr hWinPosInfo);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const UInt32 SW_HIDE = 0;
        const UInt32 SW_SHOWNORMAL = 1;
        const UInt32 SW_NORMAL = 1;
        const UInt32 SW_SHOWMINIMIZED = 2;
        const UInt32 SW_SHOWMAXIMIZED = 3;
        const UInt32 SW_MAXIMIZE = 3;
        const UInt32 SW_SHOWNOACTIVATE = 4;
        const UInt32 SW_SHOW = 5;
        const UInt32 SW_MINIMIZE = 6;
        const UInt32 SW_SHOWMINNOACTIVE = 7;
        const UInt32 SW_SHOWNA = 8;
        const UInt32 SW_RESTORE = 9;

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOZORDER = 0x0004;
        const UInt32 SWP_NOREDRAW = 0x0008;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_FRAMECHANGED = 0x0020;
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        const UInt32 SWP_NOCOPYBITS = 0x0100;
        const UInt32 SWP_NOOWNERZORDER = 0x0200;
        const UInt32 SWP_NOSENDCHANGING = 0x0400;
        const UInt32 SWP_ASYNCWINDOWPOS = 0x4000;

        private static string getWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            var builder = new StringBuilder(length + 1);

            GetWindowText(hWnd, builder, builder.Capacity);

            return builder.ToString();
        }

        private static WINDOWPLACEMENT getWindowPlacement(IntPtr hWnd)
        {
            var placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);

            GetWindowPlacement(hWnd, ref placement);

            return placement;
        }

        #endregion

        private class WindowState
        {
            public IntPtr WindowHandle { get; set; }

            public WINDOWPLACEMENT WindowPlacement { get; set; }

            public override bool Equals(object obj)
            {
                WindowState right = obj as WindowState;

                return right != null && WindowHandle == right.WindowHandle;
            }

            public override int GetHashCode()
            {
                return WindowHandle.ToInt32();
            }
        }

        private IList<WindowState> _windows;

        public bool StoreVisibleWindowStates()
        {
            bool success = false;

            _windows = getCurrentTopLevelWindows(ref success);

            return success;
        }

        private IList<WindowState> getCurrentTopLevelWindows(ref bool success)
        {
            var excludedWindowTitles = new List<string> { "Start" };
            var windows = new List<WindowState>();

            EnumDelegate enumFunc = new EnumDelegate((hWnd, lParam) =>
            {
                string windowTitle = getWindowTitle(hWnd);

                if (IsWindow(hWnd) && IsWindowVisible(hWnd) && !string.IsNullOrEmpty(windowTitle) && !excludedWindowTitles.Contains(windowTitle))
                {
                    var windowState = new WindowState
                    {
                        WindowHandle = hWnd,
                        WindowPlacement = getWindowPlacement(hWnd)
                    };

                    windows.Add(windowState);
                }

                return true;
            });

            success = EnumDesktopWindows(IntPtr.Zero, enumFunc, IntPtr.Zero);

            return windows;
        }

        public bool RestoreStoredWindowStates()
        {
            bool getWindowSuccess = false;
            IList<WindowState> currentTopLevelWindows = getCurrentTopLevelWindows(ref getWindowSuccess);

            if (_windows == null || _windows.Count == 0 || !getWindowSuccess)
            {
                return false;
            }

            // get a list of windows to restore, ordered as follows:
            //      1) windows in the saved state that still exist
            //      2) new windows opened since the state was saved
            IEnumerable<WindowState> orderedWindows =
                _windows
                    .Where(window => currentTopLevelWindows.Contains(window))
                    .Union(
                        currentTopLevelWindows.Where(window => !_windows.Contains(window))
                    );

            IntPtr windowStructureHandle = BeginDeferWindowPos(_windows.Count());
            IntPtr lastWindowHandle = IntPtr.Zero;

            foreach (var windowState in orderedWindows)
            {
                WINDOWPLACEMENT placement = windowState.WindowPlacement;

                SetWindowPlacement(windowState.WindowHandle, ref placement);
                windowStructureHandle = DeferWindowPos(windowStructureHandle, windowState.WindowHandle, lastWindowHandle, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);

                lastWindowHandle = windowState.WindowHandle;
            }

            bool success = EndDeferWindowPos(windowStructureHandle);

            // focus/activate the top window
            SetForegroundWindow(orderedWindows.First().WindowHandle);

            return success;
        }
    }
}
