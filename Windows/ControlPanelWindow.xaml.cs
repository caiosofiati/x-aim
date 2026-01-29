using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CrosshairOverlay.Controls;
using CrosshairOverlay.Models;
using CrosshairOverlay.ViewModels;

namespace CrosshairOverlay.Windows
{
    /// <summary>
    /// Control panel window for customizing the crosshair.
    /// Provides tabs for shape, color, lines, and profiles.
    /// </summary>
    public partial class ControlPanelWindow : Window
    {
        public ControlPanelWindow()
        {
            InitializeComponent();

            // Closing the control panel closes the entire application
            Closed += ControlPanelWindow_Closed;

            // Set up tab switching
            TabShape.Checked += (s, e) => SwitchTab(0);
            TabColor.Checked += (s, e) => SwitchTab(1);
            TabProfiles.Checked += (s, e) => SwitchTab(2);
        }

        /// <summary>
        /// Switches to the specified tab panel.
        /// </summary>
        private void SwitchTab(int tabIndex)
        {
            // Hide all panels
            PanelShape.Visibility = Visibility.Collapsed;
            PanelColor.Visibility = Visibility.Collapsed;
            PanelProfiles.Visibility = Visibility.Collapsed;

            // Show the selected panel
            switch (tabIndex)
            {
                case 0:
                    PanelShape.Visibility = Visibility.Visible;
                    break;
                case 1:
                    PanelColor.Visibility = Visibility.Visible;
                    break;
                case 2:
                    PanelProfiles.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// When the control panel is closed, shut down the application.
        /// </summary>
        private void ControlPanelWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles quick color button clicks.
        /// </summary>
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string colorHex)
            {
                if (DataContext is CrosshairViewModel viewModel)
                {
                    viewModel.ColorHex = colorHex;
                }
            }
        }

        /// <summary>
        /// Handles color picker color changes - syncs with ViewModel.
        /// </summary>
        private void ColorPicker_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            if (sender is ColorPickerControl picker && DataContext is CrosshairViewModel viewModel)
            {
                var color = picker.SelectedColor;
                viewModel.ColorHex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        /// <summary>
        /// Resets all settings to defaults.
        /// </summary>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CrosshairViewModel viewModel)
            {
                viewModel.ApplySettings(new CrosshairSettings());
            }
        }
    }
}
