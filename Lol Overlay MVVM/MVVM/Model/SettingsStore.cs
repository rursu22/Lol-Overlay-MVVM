using Lol_Overlay_MVVM.MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class SettingsStore : ISettingsStore
    {
        private readonly string _filePath;

        public SettingsStore()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes", "theme.json");
        }

        public async Task ModifyTheme(string newTheme)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            Dictionary<string, string> dict = new();
            if (File.Exists(_filePath))
            {
                try
                {
                    await using var inStream = File.OpenRead(_filePath);
                    var existing = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(inStream);
                    if (existing is not null)
                        dict = existing;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SettingsStore: failed reading existing themes: {ex.Message}");
                }
            }

            dict["currentTheme"] = newTheme;
            await using var outStream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(outStream, dict, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<string?> RetrieveTheme()
        {
            if (!File.Exists(_filePath))
                return null;

            try
            {
                await using var inStream = File.OpenRead(_filePath);
                var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(inStream);
                if (dict != null && dict.TryGetValue("currentTheme", out var theme))
                    return theme;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SettingsStore: failed loading theme: {ex.Message}");
            }

            return null;
        }
    }
}
