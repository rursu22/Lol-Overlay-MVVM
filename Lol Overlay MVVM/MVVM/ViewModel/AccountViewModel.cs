using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public class AccountViewModel
    {
        public string Username { get; }

        public string Password { get; }

        public IRelayCommand SelectAccountCommand { get; }
        public IRelayCommand RemoveAccountCommand { get; }
        public IRelayCommand RelogAccountCommand { get; }

        public AccountViewModel(string username, string password,Action<AccountViewModel> onSelect, Action<AccountViewModel> onRemove, Action<AccountViewModel> onRelog)
        {
            Username = username;
            Password = password;
            SelectAccountCommand = new RelayCommand(() => onSelect(this));
            RemoveAccountCommand = new RelayCommand(() => onRemove(this));
            RelogAccountCommand = new RelayCommand(() => onRelog(this));

        }
    }
}
