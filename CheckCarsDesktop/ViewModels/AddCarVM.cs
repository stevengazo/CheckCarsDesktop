using CheckCarsDesktop.Models;
using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Data;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckCarsDesktop.ViewModels
{
    public class AddCarVM : ViewModelBase
    {
        public AddCarVM() 
        {
            SaveCommand = new RelayCommand(() => { SaveCar(); });
            aPIService.Token = SharedData.Token;
            CloseCommand = new RelayCommand(ExecuteCloseCommand);
        }

        #region Actions 
        public Action CloseWindowAction { get; set; }
        #endregion

        #region Properties
        private readonly APIService aPIService = new();
        private Car _Car =new();
		public Car Car
		{
			get { return _Car; }
			set
            {
                if (_Car != value) // Verifica si el valor ha cambiado
                {
                    _Car = value;
                    OnPropertyChanged(nameof(Car));
                }
            }
		}
        #endregion
      
        #region Commands
        public RelayCommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; }

        #endregion

        #region Methods
        public async Task SaveCar()
        {
            try
            {
                if( !AreFieldsEmpty() )
                {
                    await aPIService.PostAsync<Car>("api/Cars", Car, TimeSpan.FromSeconds(10));
                    CloseCommand.Execute(null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);   
                throw;
            }

        }
        public bool AreFieldsEmpty()
        {
            return string.IsNullOrWhiteSpace(Car.Model) ||
                   string.IsNullOrWhiteSpace(Car.Brand) ||
                   string.IsNullOrWhiteSpace(Car.Type) ||
                   string.IsNullOrWhiteSpace(Car.FuelType) ||
                   string.IsNullOrWhiteSpace(Car.Plate) ||
                   string.IsNullOrWhiteSpace(Car.VIN) ||
                   Car.AdquisitionDate == null ||
                   string.IsNullOrWhiteSpace(Car.Color) || 
                   string.IsNullOrWhiteSpace(Car.Notes);
        }
        private void ExecuteCloseCommand()
        {
            CloseWindowAction?.Invoke();
        }
        #endregion


    }
}
