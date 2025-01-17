using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CheckCarsDesktop.ViewModels
{
    public class ForgotPasswordVM : ViewModelBase
    {
        private readonly APIService apIService = new APIService();

        public ICommand ResetPassword { get; set; }
        public ICommand CloseWindowCommand { get; set; }


        private string _token;
        private string _password;
        private string _confirmPassword;
        public string Token
        {
            get { return _token; }
            set
            {
                if (_token != value) // Verifica si el valor ha cambiado
                {
                    _token = value;
                    OnPropertyChanged(nameof(Token));
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
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword != value) // Verifica si el valor ha cambiado
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));

                }
            }
        }


        public ForgotPasswordVM()
        {
            ResetPassword = new RelayCommand(ChangePasswordAsync);
            CloseWindowCommand = new RelayCommand(CloseWindow);
        }

        private async void ChangePasswordAsync(object? obj)
        {
            // Verifica si la contraseña y la confirmación coinciden
            if (Password == ConfirmPassword)
            {
                // Crea el objeto de solicitud
                var request = new
                {
                    email = "steven.gazo@grupomecsa.net",
                    token = Token,
                    password = Password
                };

                // Realiza la solicitud HTTP asincrónica para cambiar la contraseña (ejemplo usando HttpClient)
                try
                {
                  var response =  await apIService.PostAsync("/api/Account/Reset", request, TimeSpan.FromSeconds(10));
                    if (response)
                    {
                        CloseWindowCommand.Execute(null);
                    }
                    
                }
                catch (Exception ex)
                {
                    // Captura cualquier excepción y muestra un mensaje de error
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }
            }
            else
            {
                // Si las contraseñas no coinciden, muestra un mensaje de error
                Console.WriteLine("Las contraseñas no coinciden. Intenta nuevamente.");
            }
        }

        private void CloseWindow(object obj)
        {
            // Aquí puedes comunicarte con la vista para cerrar la ventana
            // Dependiendo de cómo esté implementado el patrón MVVM en tu aplicación, esto puede variar.
            // A continuación, se muestra un ejemplo con un evento de la vista:

            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}

