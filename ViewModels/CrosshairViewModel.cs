using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using CrosshairOverlay.Models;
using CrosshairOverlay.Services;

namespace CrosshairOverlay.ViewModels
{
    /// <summary>
    /// Main ViewModel for the crosshair overlay application.
    /// Both windows bind to the same instance of this ViewModel.
    /// Implements INotifyPropertyChanged for real-time UI updates.
    /// </summary>
    public class CrosshairViewModel : INotifyPropertyChanged
    {
        private readonly SettingsService _settingsService;

        #region Private Fields
        private string _colorHex = "#00FF00";
        private double _size = 20;
        private double _thickness = 2;
        private double _gap = 4;
        private double _opacity = 1.0;
        private bool _showDot = true;
        private bool _showCircle = false;
        private double _circleRadius = 15;
        private double _dotSize = 4;
        private string _newPresetName = string.Empty;
        private Preset? _selectedPreset;
        private bool _crosshairEnabled = true;
        private bool _showTShape = false;
        private bool _showHorizontalLines = true;
        private bool _showVerticalLines = true;
        #endregion

        #region Properties

        /// <summary>
        /// Crosshair color as hex string (e.g., "#FF0000").
        /// </summary>
        public string ColorHex
        {
            get => _colorHex;
            set
            {
                if (_colorHex != value)
                {
                    _colorHex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CrosshairBrush));
                    OnPropertyChanged(nameof(PreviewColor));
                    // Update RGB values without triggering circular updates
                    UpdateRgbFromHex(false);
                }
            }
        }

        /// <summary>
        /// Brush for crosshair rendering, derived from ColorHex.
        /// </summary>
        public SolidColorBrush CrosshairBrush
        {
            get
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(_colorHex);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    return new SolidColorBrush(Colors.Lime);
                }
            }
        }

        /// <summary>
        /// Color for preview binding (System.Windows.Media.Color).
        /// </summary>
        public Color PreviewColor
        {
            get
            {
                try
                {
                    return (Color)ColorConverter.ConvertFromString(_colorHex);
                }
                catch
                {
                    return Colors.Lime;
                }
            }
        }

        #region RGB Color Properties

        private byte _colorRed = 0;
        private byte _colorGreen = 255;
        private byte _colorBlue = 0;
        private bool _isUpdatingRgb = false;

        /// <summary>
        /// Red component of the color (0-255).
        /// </summary>
        public double ColorRed
        {
            get => _colorRed;
            set
            {
                var byteValue = (byte)Math.Clamp(value, 0, 255);
                if (_colorRed != byteValue)
                {
                    _colorRed = byteValue;
                    OnPropertyChanged();
                    UpdateHexFromRgb();
                }
            }
        }

        /// <summary>
        /// Green component of the color (0-255).
        /// </summary>
        public double ColorGreen
        {
            get => _colorGreen;
            set
            {
                var byteValue = (byte)Math.Clamp(value, 0, 255);
                if (_colorGreen != byteValue)
                {
                    _colorGreen = byteValue;
                    OnPropertyChanged();
                    UpdateHexFromRgb();
                }
            }
        }

        /// <summary>
        /// Blue component of the color (0-255).
        /// </summary>
        public double ColorBlue
        {
            get => _colorBlue;
            set
            {
                var byteValue = (byte)Math.Clamp(value, 0, 255);
                if (_colorBlue != byteValue)
                {
                    _colorBlue = byteValue;
                    OnPropertyChanged();
                    UpdateHexFromRgb();
                }
            }
        }

        /// <summary>
        /// Updates hex color from RGB values.
        /// </summary>
        private void UpdateHexFromRgb()
        {
            if (_isUpdatingRgb) return;
            
            _colorHex = $"#{_colorRed:X2}{_colorGreen:X2}{_colorBlue:X2}";
            OnPropertyChanged(nameof(ColorHex));
            OnPropertyChanged(nameof(CrosshairBrush));
            OnPropertyChanged(nameof(PreviewColor));
        }

        /// <summary>
        /// Updates RGB values from hex color.
        /// </summary>
        private void UpdateRgbFromHex(bool notify = true)
        {
            _isUpdatingRgb = true;
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(_colorHex);
                _colorRed = color.R;
                _colorGreen = color.G;
                _colorBlue = color.B;
                
                if (notify)
                {
                    OnPropertyChanged(nameof(ColorRed));
                    OnPropertyChanged(nameof(ColorGreen));
                    OnPropertyChanged(nameof(ColorBlue));
                }
            }
            catch
            {
                // Keep existing values if conversion fails
            }
            finally
            {
                _isUpdatingRgb = false;
            }
        }

        #endregion

        /// <summary>
        /// Length of each crosshair line in pixels.
        /// </summary>
        public double Size
        {
            get => _size;
            set
            {
                if (_size != value)
                {
                    _size = Math.Clamp(value, 1, 100);
                    OnPropertyChanged();
                    UpdateLinePositions();
                }
            }
        }

        /// <summary>
        /// Thickness of crosshair lines in pixels.
        /// </summary>
        public double Thickness
        {
            get => _thickness;
            set
            {
                if (_thickness != value)
                {
                    _thickness = Math.Clamp(value, 1, 20);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gap between center and crosshair lines in pixels.
        /// </summary>
        public double Gap
        {
            get => _gap;
            set
            {
                if (_gap != value)
                {
                    _gap = Math.Clamp(value, 0, 50);
                    OnPropertyChanged();
                    UpdateLinePositions();
                }
            }
        }

        /// <summary>
        /// Overall opacity of the crosshair (0.0 - 1.0).
        /// </summary>
        public double Opacity
        {
            get => _opacity;
            set
            {
                if (_opacity != value)
                {
                    _opacity = Math.Clamp(value, 0.1, 1.0);
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Opacity as percentage for UI display (0 - 100).
        /// </summary>
        public double OpacityPercent
        {
            get => _opacity * 100;
            set => Opacity = value / 100.0;
        }

        /// <summary>
        /// Whether the crosshair overlay is enabled/visible.
        /// </summary>
        public bool CrosshairEnabled
        {
            get => _crosshairEnabled;
            set
            {
                if (_crosshairEnabled != value)
                {
                    _crosshairEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to show the center dot.
        /// </summary>
        public bool ShowDot
        {
            get => _showDot;
            set
            {
                if (_showDot != value)
                {
                    _showDot = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to show the circle around the crosshair.
        /// </summary>
        public bool ShowCircle
        {
            get => _showCircle;
            set
            {
                if (_showCircle != value)
                {
                    _showCircle = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to show T-Shape (only bottom line, no top line).
        /// </summary>
        public bool ShowTShape
        {
            get => _showTShape;
            set
            {
                if (_showTShape != value)
                {
                    _showTShape = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowTopLine));
                }
            }
        }

        /// <summary>
        /// Whether to show horizontal lines (left/right).
        /// </summary>
        public bool ShowHorizontalLines
        {
            get => _showHorizontalLines;
            set
            {
                if (_showHorizontalLines != value)
                {
                    _showHorizontalLines = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Whether to show vertical lines (top/bottom).
        /// </summary>
        public bool ShowVerticalLines
        {
            get => _showVerticalLines;
            set
            {
                if (_showVerticalLines != value)
                {
                    _showVerticalLines = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowTopLine));
                }
            }
        }

        /// <summary>
        /// Computed property: Show top line only if T-Shape is disabled and vertical lines are enabled.
        /// </summary>
        public bool ShowTopLine => !ShowTShape && ShowVerticalLines;

        /// <summary>
        /// Radius of the circle in pixels.
        /// </summary>
        public double CircleRadius
        {
            get => _circleRadius;
            set
            {
                if (_circleRadius != value)
                {
                    _circleRadius = Math.Clamp(value, 5, 100);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CircleDiameter));
                    OnPropertyChanged(nameof(CircleLeft));
                    OnPropertyChanged(nameof(CircleTop));
                }
            }
        }

        /// <summary>
        /// Size of the center dot in pixels.
        /// </summary>
        public double DotSize
        {
            get => _dotSize;
            set
            {
                if (_dotSize != value)
                {
                    _dotSize = Math.Clamp(value, 1, 20);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DotLeft));
                    OnPropertyChanged(nameof(DotTop));
                }
            }
        }

        #endregion

        #region Calculated Properties for Line Positions

        // These properties calculate the start/end positions for each crosshair line
        // The canvas is 200x200, centered at (100, 100)
        private const double CanvasCenter = 100;

        /// <summary>Top line - starts from center-gap going up</summary>
        public double TopLineY1 => CanvasCenter - Gap;
        public double TopLineY2 => CanvasCenter - Gap - Size;

        /// <summary>Bottom line - starts from center+gap going down</summary>
        public double BottomLineY1 => CanvasCenter + Gap;
        public double BottomLineY2 => CanvasCenter + Gap + Size;

        /// <summary>Left line - starts from center-gap going left</summary>
        public double LeftLineX1 => CanvasCenter - Gap;
        public double LeftLineX2 => CanvasCenter - Gap - Size;

        /// <summary>Right line - starts from center+gap going right</summary>
        public double RightLineX1 => CanvasCenter + Gap;
        public double RightLineX2 => CanvasCenter + Gap + Size;

        /// <summary>Center dot position (top-left corner)</summary>
        public double DotLeft => CanvasCenter - DotSize / 2;
        public double DotTop => CanvasCenter - DotSize / 2;

        /// <summary>Circle position (top-left corner)</summary>
        public double CircleLeft => CanvasCenter - CircleRadius;
        public double CircleTop => CanvasCenter - CircleRadius;
        public double CircleDiameter => CircleRadius * 2;

        private void UpdateLinePositions()
        {
            OnPropertyChanged(nameof(TopLineY1));
            OnPropertyChanged(nameof(TopLineY2));
            OnPropertyChanged(nameof(BottomLineY1));
            OnPropertyChanged(nameof(BottomLineY2));
            OnPropertyChanged(nameof(LeftLineX1));
            OnPropertyChanged(nameof(LeftLineX2));
            OnPropertyChanged(nameof(RightLineX1));
            OnPropertyChanged(nameof(RightLineX2));
        }

        #endregion

        #region Preset Management

        /// <summary>
        /// Collection of saved presets.
        /// </summary>
        public ObservableCollection<Preset> Presets { get; } = new();

        /// <summary>
        /// Currently selected preset in the dropdown.
        /// </summary>
        public Preset? SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                if (_selectedPreset != value)
                {
                    _selectedPreset = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Name for a new preset to save.
        /// </summary>
        public string NewPresetName
        {
            get => _newPresetName;
            set
            {
                if (_newPresetName != value)
                {
                    _newPresetName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Command to save current settings as a new preset.
        /// </summary>
        public RelayCommand SavePresetCommand { get; }

        /// <summary>
        /// Command to load the selected preset.
        /// </summary>
        public RelayCommand LoadPresetCommand { get; }

        /// <summary>
        /// Command to delete the selected preset.
        /// </summary>
        public RelayCommand DeletePresetCommand { get; }

        private void SavePreset()
        {
            if (string.IsNullOrWhiteSpace(NewPresetName)) return;

            var preset = new Preset(NewPresetName.Trim(), GetCurrentSettings());
            
            // Remove existing preset with same name
            var existing = Presets.FirstOrDefault(p => p.Name == preset.Name);
            if (existing != null)
            {
                Presets.Remove(existing);
            }

            Presets.Add(preset);
            _settingsService.SavePresets(Presets.ToList());
            
            SelectedPreset = preset;
            NewPresetName = string.Empty;
        }

        private void LoadPreset()
        {
            if (SelectedPreset == null) return;

            ApplySettings(SelectedPreset.Settings);
        }

        private void DeletePreset()
        {
            if (SelectedPreset == null) return;

            Presets.Remove(SelectedPreset);
            _settingsService.SavePresets(Presets.ToList());
            SelectedPreset = null;
        }

        #endregion

        #region Constructor

        public CrosshairViewModel(CrosshairSettings settings, List<Preset> presets, SettingsService settingsService)
        {
            _settingsService = settingsService;

            // Initialize commands
            SavePresetCommand = new RelayCommand(SavePreset, () => !string.IsNullOrWhiteSpace(NewPresetName));
            LoadPresetCommand = new RelayCommand(LoadPreset, () => SelectedPreset != null);
            DeletePresetCommand = new RelayCommand(DeletePreset, () => SelectedPreset != null);

            // Load presets
            foreach (var preset in presets)
            {
                Presets.Add(preset);
            }

            // Apply loaded settings
            ApplySettings(settings);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies settings from a CrosshairSettings object to this ViewModel.
        /// </summary>
        public void ApplySettings(CrosshairSettings settings)
        {
            ColorHex = settings.Color;
            UpdateRgbFromHex(true); // Sync RGB sliders with loaded color
            Size = settings.Size;
            Thickness = settings.Thickness;
            Gap = settings.Gap;
            Opacity = settings.Opacity;
            ShowDot = settings.ShowDot;
            ShowCircle = settings.ShowCircle;
            CircleRadius = settings.CircleRadius;
            DotSize = settings.DotSize;
        }

        /// <summary>
        /// Gets the current settings as a CrosshairSettings object.
        /// </summary>
        public CrosshairSettings GetCurrentSettings()
        {
            return new CrosshairSettings
            {
                Color = ColorHex,
                Size = Size,
                Thickness = Thickness,
                Gap = Gap,
                Opacity = Opacity,
                ShowDot = ShowDot,
                ShowCircle = ShowCircle,
                CircleRadius = CircleRadius,
                DotSize = DotSize
            };
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
