using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class ThemeService : IThemeService
    {
        private readonly string[] _themePaths;
        private int _currentIndex;
        public ThemeService()
        {
            var themeDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Themes");
            _themePaths = Directory.Exists(themeDir)
                ? Directory.GetFiles(themeDir, "*.xaml")
                : Array.Empty<string>();
            _currentIndex = 0;
        }

        public void LoadInitialTheme()
        {
            if (_themePaths.Length > 0)
                Apply(_themePaths[_currentIndex]);
        }

        public void CycleTheme()
        {
            if (_themePaths.Length == 0) return;
            _currentIndex = (_currentIndex + 1) % _themePaths.Length;
            Apply(_themePaths[_currentIndex]);
        }

        private void Apply(string path)
        {
            if (!File.Exists(path)) return;
            try
            {
                var resourceDictionary = new ResourceDictionary { Source = new Uri(path, UriKind.Absolute) };
                var merged = Application.Current.Resources.MergedDictionaries;
                merged.RemoveAt(0);
                merged.Insert(0, resourceDictionary);
            }
            catch {}
        }
    }
}