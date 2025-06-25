using Lol_Overlay_MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<T>() where T : ViewModel;
}

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public class NavigationService : Core.ViewModel, INavigationService
    {
        private Core.ViewModel _currentView;
        private readonly Func<Type, Core.ViewModel> _viewModelFactory;

        public Core.ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, Core.ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>() where TViewModel : Core.ViewModel
        {
            Core.ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
