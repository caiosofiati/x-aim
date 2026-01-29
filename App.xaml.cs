using System.Windows;
using System.Windows.Media;
using CrosshairOverlay.Services;
using CrosshairOverlay.ViewModels;
using CrosshairOverlay.Windows;

namespace CrosshairOverlay
{
    /// <summary>
    /// Application entry point. Manages shared ViewModel and both windows.
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // Force software rendering to ensure consistent colors between Debug and Release
            // Hardware rendering can produce different results in self-contained builds
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
        }
        // Shared ViewModel instance - both windows bind to this
        private CrosshairViewModel? _viewModel;
        private SettingsService? _settingsService;
        private OverlayWindow? _overlayWindow;
        private ControlPanelWindow? _controlPanelWindow;

        /// <summary>
        /// Application startup - initializes ViewModel, loads settings, and creates windows.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Initialize services
            _settingsService = new SettingsService();
            
            // Load saved settings or use defaults
            var settings = _settingsService.LoadSettings();
            var presets = _settingsService.LoadPresets();
            
            // Create shared ViewModel
            _viewModel = new CrosshairViewModel(settings, presets, _settingsService);
            
            // Create and show the overlay window (transparent, click-through)
            _overlayWindow = new OverlayWindow
            {
                DataContext = _viewModel
            };
            _overlayWindow.Show();
            
            // Create and show the control panel window
            _controlPanelWindow = new ControlPanelWindow
            {
                DataContext = _viewModel
            };
            _controlPanelWindow.Show();
        }

        /// <summary>
        /// Application exit - saves current settings.
        /// </summary>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Save settings on exit
            if (_viewModel != null && _settingsService != null)
            {
                _settingsService.SaveSettings(_viewModel.GetCurrentSettings());
            }
        }
    }
}
