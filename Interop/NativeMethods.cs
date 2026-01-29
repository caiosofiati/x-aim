using System.Runtime.InteropServices;

namespace CrosshairOverlay.Interop
{
    /// <summary>
    /// Win32 API declarations for window manipulation.
    /// Used to make the overlay window click-through.
    /// </summary>
    public static class NativeMethods
    {
        #region Constants

        /// <summary>
        /// Index for extended window styles.
        /// </summary>
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// Window should not be painted until siblings beneath it have been painted.
        /// </summary>
        public const int WS_EX_TRANSPARENT = 0x00000020;

        /// <summary>
        /// Window is a "tool window" - not shown in taskbar or Alt+Tab.
        /// </summary>
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        /// <summary>
        /// Window should be layered (required for transparency).
        /// </summary>
        public const int WS_EX_LAYERED = 0x00080000;

        #endregion

        #region P/Invoke Declarations

        /// <summary>
        /// Retrieves information about the specified window.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// Changes an attribute of the specified window.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        #region Helper Methods

        /// <summary>
        /// Makes a window click-through by adding WS_EX_TRANSPARENT style.
        /// Mouse events will pass through to windows below.
        /// </summary>
        /// <param name="hwnd">Handle to the window.</param>
        public static void MakeWindowClickThrough(IntPtr hwnd)
        {
            // Get current extended style
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            // Add transparent style (click-through)
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        /// <summary>
        /// Makes a window a tool window (hidden from taskbar and Alt+Tab).
        /// </summary>
        /// <param name="hwnd">Handle to the window.</param>
        public static void MakeWindowToolWindow(IntPtr hwnd)
        {
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TOOLWINDOW);
        }

        /// <summary>
        /// Applies all necessary styles for an overlay window:
        /// - Click-through (WS_EX_TRANSPARENT)
        /// - Tool window (not in taskbar)
        /// </summary>
        /// <param name="hwnd">Handle to the window.</param>
        public static void ApplyOverlayStyles(IntPtr hwnd)
        {
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        }

        #endregion
    }
}
