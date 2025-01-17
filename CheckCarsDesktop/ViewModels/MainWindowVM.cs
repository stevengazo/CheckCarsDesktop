using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Commands;
using CheckCarsDesktop.Views;
using CheckCarsDesktop.Views.Shared;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckCarsDesktop.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly UserCredentialsStorage _storage = new UserCredentialsStorage();
        private readonly APIService aPIService = new APIService();
        public ICommand LoginCommand { get; }

        public ICommand ResetPasswordCommand { get; }

        private string _email;
        private string _password;
        private bool _Remember;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value) // Verifica si el valor ha cambiado
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value) // Verifica si el valor ha cambiado
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public bool Remember
        {
            get { return _Remember; }
            set
            {
                if (_Remember != value) // Verifica si el valor ha cambiado
                {
                    _Remember = value;
                    OnPropertyChanged(nameof(Remember));
                }
            }
        }


        public MainWindowVM()
        {
            LoginCommand = new RelayCommand(LoginAsync);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
        }

        private async void ResetPassword(object? obj)
        {
            var inputDialog = new InputDialog("Recuperación de Contraseña", "Ingresa tu correo electrónico:");
            bool? result = inputDialog.ShowDialog();

            if (result == true)
            {

                var email = inputDialog.InputText.Split('@').ToArray();
                await aPIService.PostAsync($"/api/Account/forgot?email={email[0]}%40{email[1]}", TimeSpan.FromSeconds(5));
                Views.ForgotPassword forgotPassword = new();
                forgotPassword.ShowDialog();
            }
        }

        private async void LoginAsync(object? obj)
        {
            var request = new
            {
                email = Email,
                password = Password
            };
            var response = await aPIService.PostAsync("/api/Account/login", request, TimeSpan.FromSeconds(4));
            if (response)
            {

                Home mainWindow = new();
                mainWindow.Show();
            }

        }
    }
}
