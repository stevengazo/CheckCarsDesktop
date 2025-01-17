﻿using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Commands;
using CheckCarsDesktop.Views;
using CheckCarsDesktop.Views.Shared;
using Microsoft.Win32;
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
        private readonly APIService aPIService = new APIService();
        public ICommand LoginCommand { get; }

        public ICommand ResetPasswordCommand { get; }

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
               
              var  email = inputDialog.InputText.Split('@').ToArray();
               await aPIService.PostAsync($"/api/Account/forgot?email={email[0]}%40{email[1]}", TimeSpan.FromSeconds(5));
                Views.ForgotPassword forgotPassword = new();
                forgotPassword.ShowDialog();
            }

           
        }

        private async void LoginAsync(object? obj)
        {
             Home mainWindow = new();
            mainWindow.Show();
        }
    }
}
