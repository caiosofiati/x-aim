using System.Windows;
using System.Windows.Interop;
using CrosshairOverlay.Interop;

namespace CrosshairOverlay.Windows
{
    /// <summary>
    /// Overlay window that displays the crosshair.
    /// Configured to be transparent, borderless, always-on-top, and click-through.
    /// </summary>
    public partial class OverlayWindow : Window
    {
        public OverlayWindow()
        {
            InitializeComponent();

            // Apply click-through behavior after window handle is available
            SourceInitialized += OverlayWindow_SourceInitialized;
            
            // Center the window precisely on the primary screen
            Loaded += OverlayWindow_Loaded;
        }

        /// <summary>
        /// Called when the window has loaded. Centers the window precisely.
        /// </summary>
        private void OverlayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CenterOnScreen();
        }

        /// <summary>
        /// Centers the overlay window precisely on the primary screen.
        /// </summary>
        private void CenterOnScreen()
        {
            // Get the primary screen dimensions
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            
            // Calculate the position to center the window
            // The crosshair is at the center of the 200x200 window (100, 100)
            // So we need to position the window so that point (100, 100) is at screen center
            Left = (screenWidth / 2) - (Width / 2);
            Top = (screenHeight / 2) - (Height / 2);
        }

        /// <summary>
        /// Called when the window handle has been created.
        /// We use this to apply Win32 extended styles for click-through behavior.
        /// </summary>
        private void OverlayWindow_SourceInitialized(object? sender, EventArgs e)
        {
            // Get the window handle
            var hwnd = new WindowInteropHelper(this).Handle;

            // Apply overlay styles (click-through + tool window)
            NativeMethods.ApplyOverlayStyles(hwnd);
        }
    }
}
