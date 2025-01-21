using CheckCarsDesktop.Services;
using CheckCarsDesktop.Views;
using CheckCarsDesktop.Views.Shared;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckCarsDesktop.Shared.Data;
using CheckCarsDesktop.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace CheckCarsDesktop.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        public MainWindowVM()
        {
            LoginCommand = new RelayCommand(LoginAsync);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
            CloseCommand = new RelayCommand(ExecuteCloseCommand);
            LoadDefault();
        }

    
        #region Actions 
        public Action CloseWindowAction { get; set; }
        #endregion
       
        #region Commands
        public RelayCommand LoginCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        #endregion

        #region Properties
        private readonly UserCredentialsStorage _storage = new UserCredentialsStorage();
        private readonly APIService aPIService = new APIService();
        private string _email;
        private string _password;
        private bool _Remember;
        private Visibility _Signing = Visibility.Hidden;
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

        public Visibility Signing
        {
            get { return _Signing; }
            set
            {
                if (_Signing != value) // Verifica si el valor ha cambiado
                {
                    _Signing = value;
                    OnPropertyChanged(nameof(Signing));
                }
            }
        }

        #endregion

        #region Methods
        private async void LoadDefault()
        {
            var user = _storage.LoadCredentials();
            if (user != null && user.SaveUserName)
            {
                Email = user.Username;
                Password = user.pass;
                Remember = user.SaveUserName;
            }
        }
        private async void ResetPassword()
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
        private  async void LoginAsync()
        {
            try
            {
                Signing = Visibility.Visible;
                var request = new
                {
                    email = Email,
                    password = Password
                };
                var response =await  aPIService.PostAsync("/api/Account/login", request, TimeSpan.FromSeconds(4));
                if (response != null)
                {
                    var token = JsonConvert.DeserializeObject<ResponseToken>(response);
                    SharedData.Token = token.Token;

                    
                    if (Remember)
                    {
                        UserCredentials userCredentials = new UserCredentials();
                        userCredentials.Username = Email;
                        userCredentials.SaveUserName= Remember;
                        userCredentials.pass = Password;
                        _storage.SaveCredentials(userCredentials);
                    }
                    else
                    {
                        _storage.ClearCredentials();
                    }


                  

                    Home mainWindow = new();
                    mainWindow.Show();
                    CloseCommand.Execute(null);
                }
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                Signing = Visibility.Hidden;    
            }

        }
        private void ExecuteCloseCommand()
        {
            CloseWindowAction?.Invoke();
        }
        #endregion
    }

    public class ResponseToken
    {
        public string Token { get; set; }
    }
}
