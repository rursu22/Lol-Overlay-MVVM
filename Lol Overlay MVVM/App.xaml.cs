using CommunityToolkit.Mvvm.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Lol_Overlay_MVVM.Core;
using Lol_Overlay_MVVM.MVVM.Interfaces;
using Lol_Overlay_MVVM.MVVM.Model;
using Lol_Overlay_MVVM.MVVM.View;
using Lol_Overlay_MVVM.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Lol_Overlay_MVVM
{
    public partial class App : System.Windows.Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<ICalibrationService, CalibrationService>();
            services.AddSingleton<IComputerVisionService, ComputerVisionService>();
            services.AddSingleton<IWindowFinderService, WindowFinderService>();
            services.AddSingleton<ISettingsStore, SettingsStore>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<INavigationService>(serviceProvider => new NavigationService(serviceProvider.GetRequiredService<Func<Type, ViewModel>>()));

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<CalibrationViewModel>();
            services.AddSingleton<AddAccountViewModel>();
            services.AddSingleton<HomeViewModel>();


            _serviceProvider = services.BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var themeService = _serviceProvider.GetRequiredService<IThemeService>();
            var themeName = await themeService.LoadInitialThemeAsync();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.ThemeName = themeName;

            var trayIcon = (TaskbarIcon)this.Resources["TrayIcon"];

            trayIcon.DataContext = mainViewModel;

            mainWindow.Show();
            
        }

    }

}
