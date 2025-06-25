// ViewModels/AddAccountViewModel.cs
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.MVVM.Model;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public partial class AddAccountViewModel : Core.ViewModel
    {
        private readonly IAccountStore _accountStore;
        private readonly INavigationService _navigation;
        private readonly IThemeService _themeService;

        public AsyncRelayCommand AddAccountCommand { get; }
        public RelayCommand BackCommand { get; }
        public RelayCommand ChangeThemeCommand { get; }

        public AddAccountViewModel(
            IAccountStore accountStore,
            INavigationService navigation,
            IThemeService themeService
            )
        {
            _accountStore = accountStore;
            _navigation = navigation;
            _themeService = themeService;

            // Commands
            AddAccountCommand = new AsyncRelayCommand(ExecuteAddAccountAsync, CanExecuteAddAccount);
            BackCommand = new RelayCommand(ExecuteBack);
            ChangeThemeCommand = new RelayCommand(ExecuteChangeTheme);
        }

        //–– bindable properties ––
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string plainPassword = string.Empty;

        [ObservableProperty]
        private bool showPassword;

        partial void OnUsernameChanged(string? oldValue, string newValue)
        {
            AddAccountCommand.NotifyCanExecuteChanged();
        }

        partial void OnPlainPasswordChanged(string? oldValue, string newValue)
        {
            AddAccountCommand.NotifyCanExecuteChanged();
        }

        private async Task ExecuteAddAccountAsync()
        {
            await _accountStore.AddAccountAsync(Username, PlainPassword);

            Username = string.Empty;
            PlainPassword = string.Empty;
            AddAccountCommand.NotifyCanExecuteChanged();

        }

        private bool CanExecuteAddAccount()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(PlainPassword);
        }

        private void ExecuteBack()
        {
            // simply go back to HomeViewModel
            _navigation.NavigateTo<HomeViewModel>();
        }

        private void ExecuteChangeTheme()
        {
            _themeService.CycleTheme();
        }
    }
}