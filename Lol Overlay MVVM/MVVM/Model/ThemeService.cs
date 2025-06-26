using Lol_Overlay_MVVM.MVVM.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class ThemeService : IThemeService
    {
        private readonly string[] _themePaths;
        private readonly ISettingsStore _settingsStore;
        private int _currentIndex;
        public ThemeService(ISettingsStore settingsStore)
        {
            _settingsStore = settingsStore;
            var themeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes");
            _themePaths = Directory.Exists(themeDir)
                ? Directory.GetFiles(themeDir, "*.xaml")
                : Array.Empty<string>();
            _currentIndex = 0;
        }

        public string GetCurrentThemeName()
        {
            return GetThemeNameFromPath(_themePaths[_currentIndex]);
        }

        private string GetThemeNameFromPath(string path)
        {
            string[] nameSplit = path.Split("\\");
            string xamlName = nameSplit[nameSplit.Length - 1];

            string actualName = xamlName.Split('.')[0];

            return actualName;
        }

        public async Task<string> LoadInitialThemeAsync()
        {
            var savedName = await _settingsStore.RetrieveTheme();

            if (!string.IsNullOrEmpty(savedName))
            {
                int idx = Array.FindIndex(_themePaths,
                    p => Path.GetFileNameWithoutExtension(p)
                             .Equals(savedName, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) _currentIndex = idx;
                else Debug.WriteLine($"LoadInitialTheme: saved '{savedName}' not found");
            }
            string themeName = Path.GetFileNameWithoutExtension(_themePaths[_currentIndex]);
            ApplyByName(themeName);

            return themeName;
        }

        public void CycleTheme()
        {
            if (_themePaths.Length == 0)
            {
                return;
            }

            _currentIndex = (_currentIndex + 1) % _themePaths.Length;


            ApplyByName(GetCurrentThemeName());
            _settingsStore.ModifyTheme(GetCurrentThemeName());
        }

        private void ApplyByName(string themeName)
        {
            var match = _themePaths
                .FirstOrDefault(p => Path.GetFileNameWithoutExtension(p)
                                        .Equals(themeName, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                return;
            }

            var rd = new ResourceDictionary { Source = new Uri(match, UriKind.Absolute) };
            var merged = System.Windows.Application.Current.Resources.MergedDictionaries;
            if (merged.Count > 0)
                merged[0] = rd;
            else
                merged.Insert(0, rd);
        }
    }
}