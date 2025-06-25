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

            AddAccountCommand = new AsyncRelayCommand(ExecuteAddAccountAsync, CanExecuteAddAccount);
        }

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string plainPassword = string.Empty;

        [ObservableProperty]
        private bool showPassword;

        [ObservableProperty]
        private bool isAdded;

        public bool IsAccountValid => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(PlainPassword);

        partial void OnUsernameChanged(string? oldValue, string newValue)
        {
            AddAccountCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(IsAccountValid));
        }

        partial void OnPlainPasswordChanged(string? oldValue, string newValue)
        {
            AddAccountCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(IsAccountValid));
        }

        private async Task ExecuteAddAccountAsync()
        {
            await _accountStore.AddAccountAsync(Username, PlainPassword);

            Username = string.Empty;
            PlainPassword = string.Empty;
            AddAccountCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(IsAccountValid));

            IsAdded = true;

            await Task.Delay(TimeSpan.FromMilliseconds(600));

            IsAdded = false;
        }

        private bool CanExecuteAddAccount()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(PlainPassword);
        }

    }
}