using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.MVVM.Interfaces;
using Lol_Overlay_MVVM.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using Lol_Overlay_MVVM.MVVM.View;

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

        public RelayCommand CycleThemeCommand { get; }

        [ObservableProperty]
        bool shouldWindowBeVisible = false;

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

            NavigateToHomeCommand = new RelayCommand(ExecuteNavigateToHome);

            NavigateToAddAccountCommand = new RelayCommand(ExecuteNavigateToAddAccount);

            NavigateToCalibrationCommand = new RelayCommand(ExecuteNavigateToCalibration);

            CycleThemeCommand = new RelayCommand(ExecuteCycleTheme);

            _navigation.NavigateTo<HomeViewModel>();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (sender, e) =>
            {
                bool leagueReady = _windowFinderService.isLeagueWindowReady();
                bool leagueFocused = _windowFinderService.isLeagueWindowFocused();
                var overlay = System.Windows.Application.Current.MainWindow;

                bool overlayFocused = overlay != null && overlay.IsKeyboardFocusWithin;

                ShouldWindowBeVisible = leagueReady && (leagueFocused || overlayFocused);
            };

            timer.Start();
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
        }
    }
}

