using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WindowsIntegrityGuard.UI.Models;

namespace WindowsIntegrityGuard.UI.Services
{
    public sealed class SettingsService : ISettingsService
    {
        private readonly string _settingsPath;
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        public SettingsService()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WindowsIntegrityGuard");
            Directory.CreateDirectory(directory);
            _settingsPath = Path.Combine(directory, "settings.json");
        }

        public async Task<AppSettings> LoadAsync()
        {
            if (!File.Exists(_settingsPath))
            {
                return new AppSettings();
            }

            try
            {
                string json = await File.ReadAllTextAsync(_settingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public async Task SaveAsync(AppSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);
            string json = JsonSerializer.Serialize(settings, _jsonOptions);
            await File.WriteAllTextAsync(_settingsPath, json);
        }
    }
}
