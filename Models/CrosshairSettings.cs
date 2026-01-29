namespace CrosshairOverlay.Models
{
    /// <summary>
    /// Data model containing all crosshair customization settings.
    /// This is serialized to/from JSON for persistence.
    /// </summary>
    public class CrosshairSettings
    {
        /// <summary>
        /// Crosshair color in hex format (e.g., "#FF0000" for red).
        /// </summary>
        public string Color { get; set; } = "#00FF00";

        /// <summary>
        /// Size (length) of each crosshair line in pixels.
        /// </summary>
        public double Size { get; set; } = 20;

        /// <summary>
        /// Thickness of the crosshair lines in pixels.
        /// </summary>
        public double Thickness { get; set; } = 2;

        /// <summary>
        /// Gap between the center and the start of each line in pixels.
        /// </summary>
        public double Gap { get; set; } = 4;

        /// <summary>
        /// Opacity of the crosshair (0.0 = transparent, 1.0 = fully opaque).
        /// </summary>
        public double Opacity { get; set; } = 1.0;

        /// <summary>
        /// Whether to show a center dot.
        /// </summary>
        public bool ShowDot { get; set; } = true;

        /// <summary>
        /// Whether to show a circle around the crosshair.
        /// </summary>
        public bool ShowCircle { get; set; } = false;

        /// <summary>
        /// Radius of the circle (when ShowCircle is true).
        /// </summary>
        public double CircleRadius { get; set; } = 15;

        /// <summary>
        /// Size of the center dot in pixels.
        /// </summary>
        public double DotSize { get; set; } = 4;

        /// <summary>
        /// Creates a deep copy of the settings.
        /// </summary>
        public CrosshairSettings Clone()
        {
            return new CrosshairSettings
            {
                Color = this.Color,
                Size = this.Size,
                Thickness = this.Thickness,
                Gap = this.Gap,
                Opacity = this.Opacity,
                ShowDot = this.ShowDot,
                ShowCircle = this.ShowCircle,
                CircleRadius = this.CircleRadius,
                DotSize = this.DotSize
            };
        }
    }
}
