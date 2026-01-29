using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CrosshairOverlay.Controls
{
    /// <summary>
    /// A custom HSV color picker with a saturation/brightness square, hue slider, and HSV controls.
    /// </summary>
    public partial class ColorPickerControl : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPickerControl),
                new FrameworkPropertyMetadata(Colors.Red, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        // Event for color changes
        public static readonly RoutedEvent SelectedColorChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedColorChanged), RoutingStrategy.Bubble, 
                typeof(RoutedEventHandler), typeof(ColorPickerControl));

        public event RoutedEventHandler SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }

        #endregion

        // HSV values (Hue 0-360, Saturation 0-1, Value 0-1)
        private double _hue = 0;
        private double _saturation = 1;
        private double _value = 1;
        private bool _isUpdating = false;
        private bool _isSliderUpdating = false;

        private bool _isInitialized = false;

        public ColorPickerControl()
        {
            InitializeComponent();
            _isInitialized = true;
            UpdateFromColor(Colors.Red);
        }

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorPickerControl)d;
            if (!picker._isUpdating)
            {
                picker.UpdateFromColor((Color)e.NewValue);
            }
        }

        private void UpdateFromColor(Color color)
        {
            RgbToHsv(color.R, color.G, color.B, out _hue, out _saturation, out _value);
            UpdateSatValGradient();
            UpdateIndicators();
            UpdateSliderTracks();
            UpdateSliderValues();
            UpdatePreviewColor();
        }

        private void UpdateSatValGradient()
        {
            // Update the saturation/value square's base color from hue
            var hueColor = HsvToColor(_hue, 1, 1);
            HueColorBrush.Color = hueColor;
        }

        private void UpdateSliderTracks()
        {
            // Update saturation track end color
            var hueColor = HsvToColor(_hue, 1, 1);
            SaturationEndColor.Color = hueColor;
            
            // Update value track end color (at current hue and saturation)
            var fullValueColor = HsvToColor(_hue, _saturation, 1);
            ValueEndColor.Color = fullValueColor;
        }

        private void UpdateSliderValues()
        {
            if (_isSliderUpdating) return;
            
            _isSliderUpdating = true;
            
            HueSliderControl.Value = _hue;
            SaturationSliderControl.Value = _saturation * 100;
            ValueSliderControl.Value = _value * 100;
            
            HueValueText.Text = $"{(int)_hue}°";
            SaturationValueText.Text = $"{(int)(_saturation * 100)}%";
            ValueValueText.Text = $"{(int)(_value * 100)}%";
            
            _isSliderUpdating = false;
        }

        private void UpdatePreviewColor()
        {
            PreviewBrush.Color = HsvToColor(_hue, _saturation, _value);
        }

        private void UpdateIndicators()
        {
            // Don't update if control hasn't been laid out yet
            if (HueSlider.ActualHeight <= 0 || SatValSquare.ActualWidth <= 0 || SatValSquare.ActualHeight <= 0)
                return;

            // Update hue slider indicator position (bar is 10px height)
            double hueY = (_hue / 360.0) * HueSlider.ActualHeight;
            Canvas.SetTop(HueIndicator, Math.Clamp(hueY - 5, 0, Math.Max(0, HueSlider.ActualHeight - 10)));

            // Update satval indicator position
            double satX = _saturation * SatValSquare.ActualWidth;
            double valY = (1 - _value) * SatValSquare.ActualHeight;
            Canvas.SetLeft(SatValIndicator, Math.Clamp(satX - 8, 0, Math.Max(0, SatValSquare.ActualWidth - 16)));
            Canvas.SetTop(SatValIndicator, Math.Clamp(valY - 8, 0, Math.Max(0, SatValSquare.ActualHeight - 16)));
        }

        private void UpdateSelectedColor()
        {
            _isUpdating = true;
            SelectedColor = HsvToColor(_hue, _saturation, _value);
            UpdatePreviewColor();
            RaiseEvent(new RoutedEventArgs(SelectedColorChangedEvent));
            _isUpdating = false;
        }

        #region Slider Event Handlers

        private void HueSliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isInitialized || _isSliderUpdating || _isUpdating) return;
            if (HueValueText == null) return;
            
            _hue = e.NewValue;
            HueValueText.Text = $"{(int)_hue}°";
            UpdateSatValGradient();
            UpdateSliderTracks();
            UpdateIndicators();
            UpdateSelectedColor();
        }

        private void SaturationSliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isInitialized || _isSliderUpdating || _isUpdating) return;
            if (SaturationValueText == null) return;
            
            _saturation = e.NewValue / 100.0;
            SaturationValueText.Text = $"{(int)e.NewValue}%";
            UpdateSliderTracks();
            UpdateIndicators();
            UpdateSelectedColor();
        }

        private void ValueSliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isInitialized || _isSliderUpdating || _isUpdating) return;
            if (ValueValueText == null) return;
            
            _value = e.NewValue / 100.0;
            ValueValueText.Text = $"{(int)e.NewValue}%";
            UpdateIndicators();
            UpdateSelectedColor();
        }

        #endregion

        #region Mouse Handlers

        private void SatValSquare_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SatValSquare.CaptureMouse();
            UpdateSatValFromMouse(e.GetPosition(SatValSquare));
        }

        private void SatValSquare_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && SatValSquare.IsMouseCaptured)
            {
                UpdateSatValFromMouse(e.GetPosition(SatValSquare));
            }
        }

        private void SatValSquare_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SatValSquare.ReleaseMouseCapture();
        }

        private void UpdateSatValFromMouse(Point pos)
        {
            _saturation = Math.Clamp(pos.X / SatValSquare.ActualWidth, 0, 1);
            _value = Math.Clamp(1 - (pos.Y / SatValSquare.ActualHeight), 0, 1);
            UpdateIndicators();
            UpdateSliderTracks();
            UpdateSliderValues();
            UpdateSelectedColor();
        }

        private void HueSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HueSlider.CaptureMouse();
            UpdateHueFromMouse(e.GetPosition(HueSlider));
        }

        private void HueSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && HueSlider.IsMouseCaptured)
            {
                UpdateHueFromMouse(e.GetPosition(HueSlider));
            }
        }

        private void HueSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HueSlider.ReleaseMouseCapture();
        }

        private void UpdateHueFromMouse(Point pos)
        {
            _hue = Math.Clamp((pos.Y / HueSlider.ActualHeight) * 360, 0, 360);
            UpdateSatValGradient();
            UpdateSliderTracks();
            UpdateSliderValues();
            UpdateIndicators();
            UpdateSelectedColor();
        }

        #endregion

        #region Color Conversion

        private static void RgbToHsv(byte r, byte g, byte b, out double h, out double s, out double v)
        {
            double rd = r / 255.0;
            double gd = g / 255.0;
            double bd = b / 255.0;

            double max = Math.Max(rd, Math.Max(gd, bd));
            double min = Math.Min(rd, Math.Min(gd, bd));
            double delta = max - min;

            v = max;
            s = max == 0 ? 0 : delta / max;

            if (delta == 0)
            {
                h = 0;
            }
            else if (max == rd)
            {
                h = 60 * (((gd - bd) / delta) % 6);
            }
            else if (max == gd)
            {
                h = 60 * (((bd - rd) / delta) + 2);
            }
            else
            {
                h = 60 * (((rd - gd) / delta) + 4);
            }

            if (h < 0) h += 360;
        }

        private static Color HsvToColor(double h, double s, double v)
        {
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            double m = v - c;

            double rd, gd, bd;

            if (h < 60) { rd = c; gd = x; bd = 0; }
            else if (h < 120) { rd = x; gd = c; bd = 0; }
            else if (h < 180) { rd = 0; gd = c; bd = x; }
            else if (h < 240) { rd = 0; gd = x; bd = c; }
            else if (h < 300) { rd = x; gd = 0; bd = c; }
            else { rd = c; gd = 0; bd = x; }

            return Color.FromRgb(
                (byte)((rd + m) * 255),
                (byte)((gd + m) * 255),
                (byte)((bd + m) * 255));
        }

        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Update indicators when size changes
            UpdateIndicators();
        }
    }
}
