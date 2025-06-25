using CommunityToolkit.Mvvm.Input;
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

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider => viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<INavigationService>(serviceProvider => new NavigationService(serviceProvider.GetRequiredService<Func<Type, ViewModel>>()));

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AccountViewModel>();
            services.AddSingleton<CalibrationViewModel>();
            services.AddSingleton<AddAccountViewModel>();
            services.AddSingleton<HomeViewModel>();


            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
            base.OnStartup(e);
        }

    }

}
