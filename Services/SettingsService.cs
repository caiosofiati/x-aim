using System.IO;
using System.Text.Json;
using CrosshairOverlay.Models;

namespace CrosshairOverlay.Services
{
    /// <summary>
    /// Service for loading and saving settings and presets to JSON files.
    /// Files are stored in the same directory as the executable.
    /// </summary>
    public class SettingsService
    {
        private readonly string _settingsPath;
        private readonly string _presetsPath;
        private readonly JsonSerializerOptions _jsonOptions;

        public SettingsService()
        {
            // Get the directory where the executable is located
            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            _settingsPath = Path.Combine(appDir, "settings.json");
            _presetsPath = Path.Combine(appDir, "presets.json");

            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        #region Settings

        /// <summary>
        /// Saves the current settings to settings.json.
        /// </summary>
        public void SaveSettings(CrosshairSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, _jsonOptions);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception ex)
            {
                // Log error but don't crash - settings persistence is not critical
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads settings from settings.json, or returns default settings if file doesn't exist.
        /// </summary>
        public CrosshairSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    var settings = JsonSerializer.Deserialize<CrosshairSettings>(json, _jsonOptions);
                    return settings ?? new CrosshairSettings();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }

            return new CrosshairSettings();
        }

        #endregion

        #region Presets

        /// <summary>
        /// Saves all presets to presets.json.
        /// </summary>
        public void SavePresets(List<Preset> presets)
        {
            try
            {
                var json = JsonSerializer.Serialize(presets, _jsonOptions);
                File.WriteAllText(_presetsPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save presets: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads all presets from presets.json.
        /// </summary>
        public List<Preset> LoadPresets()
        {
            try
            {
                if (File.Exists(_presetsPath))
                {
                    var json = File.ReadAllText(_presetsPath);
                    var presets = JsonSerializer.Deserialize<List<Preset>>(json, _jsonOptions);
                    return presets ?? new List<Preset>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load presets: {ex.Message}");
            }

            return new List<Preset>();
        }

        #endregion
    }
}
