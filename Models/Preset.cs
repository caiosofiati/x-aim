namespace CrosshairOverlay.Models
{
    /// <summary>
    /// Represents a named preset containing crosshair settings.
    /// </summary>
    public class Preset
    {
        /// <summary>
        /// Display name of the preset.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The crosshair settings for this preset.
        /// </summary>
        public CrosshairSettings Settings { get; set; } = new();

        public Preset() { }

        public Preset(string name, CrosshairSettings settings)
        {
            Name = name;
            Settings = settings.Clone();
        }
    }
}
