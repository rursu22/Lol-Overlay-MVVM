using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public partial class HomeViewModel : Core.ViewModel
    {
        // Properties and services

        [ObservableProperty]
        public ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();

        [ObservableProperty]
        private int columns = 3;
        [ObservableProperty]
        private int rows = 3;

        private readonly IAccountStore _accountStore;
        private readonly INavigationService _navigationService;
        private readonly IEncryptionService _encryptionService;
        private readonly ILoginService _loginService;



        // Commands
        public IRelayCommand<AccountViewModel> SelectAccountCommand { get; }
        public IRelayCommand<AccountViewModel> RemoveAccountCommand { get; }
        public IRelayCommand NavigateToAddAccountCommand { get; }

        public IRelayCommand NavigateToCalibrationCommand { get; }

        public IAsyncRelayCommand LoadAccountsCommand { get; }


        public HomeViewModel(IAccountStore accountStore, INavigationService navigationService, ILoginService loginService, IEncryptionService encryptionService)
        {
            _accountStore = accountStore;
            _navigationService = navigationService;
            _loginService = loginService;
            _encryptionService = encryptionService;

            SelectAccountCommand = new RelayCommand<AccountViewModel>(ExecuteSelectAccount);
            RemoveAccountCommand = new RelayCommand<AccountViewModel>(ExecuteRemoveAccount);
            NavigateToAddAccountCommand = new RelayCommand(ExecuteNavigateToAddAccount);
            NavigateToCalibrationCommand = new RelayCommand(ExecuteNavigateToCalibration);
            LoadAccountsCommand = new AsyncRelayCommand(ExecuteLoadAccounts);

        }

        // Execute methods

        private async Task ExecuteLoadAccounts()
        {
            var accountsList = await _accountStore.GetAccountsAsync();
            Accounts.Clear();

            foreach (var account in accountsList)
            {
                Accounts.Add(new AccountViewModel(
                    account.Key,
                    account.Value,
                    ExecuteSelectAccount,
                    ExecuteRemoveAccount));
            }

            while( Accounts.Count < (Rows * Columns))
            {
                Accounts.Add(new AccountViewModel(
                    "",
                    "",
                    ExecuteSelectAccount, ExecuteRemoveAccount));
            }
        }

        private void ExecuteSelectAccount(AccountViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Username) && !string.IsNullOrEmpty(viewModel.Password))
            {
                string password = _encryptionService.Decrypt(viewModel.Password);

                _loginService.clickUsernameAndType(viewModel.Username);
                _loginService.clickPasswordAndType(password);
                _loginService.clickLogin();
            }
        }

        private async void ExecuteRemoveAccount(AccountViewModel viewModel)
        {
            if(!string.IsNullOrEmpty(viewModel.Username))
            {
                await _accountStore.RemoveAccountAsync(viewModel.Username);
                await ExecuteLoadAccounts();
            }
            
        }

        private void ExecuteNavigateToAddAccount()
        {
            _navigationService.NavigateTo<AddAccountViewModel>();
        }

        private void ExecuteNavigateToCalibration()
        {
            _navigationService.NavigateTo<CalibrationViewModel>();
        }
    }
}
