using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.MVVM.Interfaces;
using Lol_Overlay_MVVM.MVVM.Model;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Threading;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public partial class MainViewModel : Core.ViewModel
    {
        public INavigationService _navigation;
        public IThemeService _themeService;
        public IWindowFinderService _windowFinderService;

        public RelayCommand NavigateToHomeCommand { get; }
        public RelayCommand NavigateToAddAccountCommand { get; }
        public RelayCommand NavigateToCalibrationCommand { get; }
        public IRelayCommand ShowOverlayCommand { get; }
        public IRelayCommand HideOverlayCommand { get; }
        public IRelayCommand ExitAppCommand { get; }
        public RelayCommand CycleThemeCommand { get; }
        public RelayCommand ToggleStartupCommand { get; }

        [ObservableProperty]
        private bool shouldWindowBeVisible = false;
        [ObservableProperty]
        private string themeName;
        [ObservableProperty]
        private bool startWithWindows;


        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(INavigationService navService, IThemeService themeService, IWindowFinderService windowFinderService)
        {
            Navigation = navService;

            _themeService = themeService;

            _windowFinderService = windowFinderService;

            StartWithWindows = IsStartupEnabled();

            NavigateToHomeCommand = new RelayCommand(ExecuteNavigateToHome);

            NavigateToAddAccountCommand = new RelayCommand(ExecuteNavigateToAddAccount);

            NavigateToCalibrationCommand = new RelayCommand(ExecuteNavigateToCalibration);

            CycleThemeCommand = new RelayCommand(ExecuteCycleTheme);

            ShowOverlayCommand = new RelayCommand(() => ShouldWindowBeVisible = true);

            HideOverlayCommand = new RelayCommand(() => ShouldWindowBeVisible = false);

            ExitAppCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());

            _navigation.NavigateTo<HomeViewModel>();

            ThemeName = _themeService.GetCurrentThemeName();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            timer.Tick += (sender, e) =>
            {
                bool leagueReady = _windowFinderService.isLeagueWindowReady();
                bool leagueOrOverlayFocused = _windowFinderService.IsLeagueOrOverlayForeground();
                ShouldWindowBeVisible = leagueReady && leagueOrOverlayFocused;
            };

            timer.Start();
        }

        partial void OnStartWithWindowsChanged(bool newValue)
        {
            ExecuteToggleStartup(newValue);
        }

        private bool IsStartupEnabled()
        {
            using var rk = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", writable: false);
            var val = rk?.GetValue("LoLAccountOverlay") as string;
            var exe = $"\"{Environment.ProcessPath}\"";
            return StringComparer.OrdinalIgnoreCase.Equals(val, exe);
        }

        private void ExecuteToggleStartup(bool enable)
        {
            using var registryKey = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", writable: true);
            const string key = "LoLAccountOverlay";
            var exe = Environment.ProcessPath;
            if (enable)
                registryKey.SetValue(key, $"\"{exe}\"");
            else
                registryKey.DeleteValue(key, throwOnMissingValue: false);
        }

        public void ExecuteNavigateToHome()
        {
            _navigation.NavigateTo<HomeViewModel>();
        }

        public void ExecuteNavigateToAddAccount()
        {
            _navigation.NavigateTo<AddAccountViewModel>();
        }

        public void ExecuteNavigateToCalibration()
        {
            _navigation.NavigateTo<CalibrationViewModel>();
        }

        public void ExecuteCycleTheme()
        {
            _themeService.CycleTheme();
            ThemeName = _themeService.GetCurrentThemeName();
        }


    }
}

