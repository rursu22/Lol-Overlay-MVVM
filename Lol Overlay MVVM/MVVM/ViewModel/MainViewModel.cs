using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        public INavigationService _navigation;
        public IThemeService _themeService;

        public RelayCommand NavigateToHomeCommand { get; }
        public RelayCommand NavigateToAddAccountCommand { get; }
        public RelayCommand NavigateToCalibrationCommand { get; }

        public RelayCommand CycleThemeCommand { get; }

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(INavigationService navService, IThemeService themeService)
        {
            Navigation = navService;
            _themeService = themeService;

            NavigateToHomeCommand = new RelayCommand(ExecuteNavigateToHome);

            NavigateToAddAccountCommand = new RelayCommand(ExecuteNavigateToAddAccount);

            NavigateToCalibrationCommand = new RelayCommand(ExecuteNavigateToCalibration);

            CycleThemeCommand = new RelayCommand(ExecuteCycleTheme);

            _navigation.NavigateTo<HomeViewModel>();
        }

        public void ExecuteNavigateToHome()
        {
            _navigation.NavigateTo<HomeViewModel>();
            Debug.WriteLine("Hello");
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

