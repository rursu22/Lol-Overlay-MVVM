using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lol_Overlay_MVVM.Core;
using Lol_Overlay_MVVM.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lol_Overlay_MVVM.MVVM.ViewModel
{
    public partial class CalibrationViewModel : Core.ViewModel
    {
        private readonly ILoginService _loginService;
        private readonly ICalibrationService _calibrationService;
        [ObservableProperty]
        private string usernamePosition = String.Empty;
        [ObservableProperty]
        private string passwordPosition = String.Empty;
        [ObservableProperty]
        private string loginPosition = String.Empty;

        public AsyncRelayCommand CalibrateUsernameCommand { get; }
        public AsyncRelayCommand CalibratePasswordCommand { get; }

        public AsyncRelayCommand CalibrateLoginCommand { get; }

        public CalibrationViewModel(ILoginService loginService, ICalibrationService calibrationService)
        {
            _loginService = loginService;
            _calibrationService = calibrationService;

            usernamePosition = $"Username Box Position: {_loginService.getUsernameBoxPosition().X} , {_loginService.getUsernameBoxPosition().Y}";
            passwordPosition = $"Password Box Position: {_loginService.getPasswordBoxPosition().X}, {_loginService.getPasswordBoxPosition().Y}";
            loginPosition = $"Login Button Position: {_loginService.getLoginButtonPosition().X}, {_loginService.getLoginButtonPosition().Y}";

            CalibrateUsernameCommand = new AsyncRelayCommand(ExecuteCalibrateUsername);
            CalibratePasswordCommand = new AsyncRelayCommand(ExecuteCalibratePassword);
            CalibrateLoginCommand = new AsyncRelayCommand(ExecuteCalibrateLogin);
        }

        private async Task ExecuteCalibrateUsername()
        {

            var window = System.Windows.Application.Current.MainWindow;
            window?.Hide();

            var newPosition = await _calibrationService.CaptureClickAsync("username box");
            _loginService.setUsernameBoxPosition(newPosition);
            UsernamePosition = $"Username Box Position: {newPosition.X}, {newPosition.Y}";

            window?.Show();
        }
        private async Task ExecuteCalibratePassword()
        {

            var window = System.Windows.Application.Current.MainWindow;
            window?.Hide();

            var newPosition = await _calibrationService.CaptureClickAsync("password box");
            _loginService.setPasswordBoxPosition(newPosition);
            PasswordPosition = $"Password Box Position: {newPosition.X}, {newPosition.Y}";

            window?.Show();
        }
        private async Task ExecuteCalibrateLogin()
        {
            var window = System.Windows.Application.Current.MainWindow;
            window?.Hide();

            var newPosition = await _calibrationService.CaptureClickAsync("login button");
            _loginService.setLoginButtonPosition(newPosition);
            LoginPosition = $"Login Button Position: {newPosition.X}, {newPosition.Y}";

            window?.Show();

        }
    }
}
