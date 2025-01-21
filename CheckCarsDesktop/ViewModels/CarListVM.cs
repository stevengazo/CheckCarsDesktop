using CheckCarsDesktop.Models;
using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Data;
using CheckCarsDesktop.Views;
using CommunityToolkit.Mvvm.Input;
using Meziantou.Framework.WPF.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.ViewModels
{
    public class CarListVM : ViewModelBase
    {
        public CarListVM()
        {
            ISeeEntryReport = new RelayCommand<string>(e => SeeEntry(e));
            ISeeCrashReport = new RelayCommand<string>(e => SeeCrash(e));
            ISeeIssueReport = new RelayCommand<string>(e => Seeissue(e));
            SelectedCarCommand = new RelayCommand<Car>(async (car) => await LoadCar(car));
            AddCarCommand = new RelayCommand(async () => await AddCar());
            _aPIService.Token = SharedData.Token;
            LoadCars();
        }

        #region Properties
        private readonly APIService _aPIService = new APIService();
        private ConcurrentObservableCollection<Car> _cars = new ConcurrentObservableCollection<Car>();
        private Car _SelectedCar = new();
        private ConcurrentObservableCollection<EntryExitReport> _Entries = new();
        private ConcurrentObservableCollection<IssueReport> _Issues = new();
        private ConcurrentObservableCollection<CrashReport> _Crashes = new();

        public ConcurrentObservableCollection<Car> Cars
        {
            get { return _cars; }
            set
            {
                if (_cars != value) // Verifica si el valor ha cambiado
                {
                    _cars = value;
                    OnPropertyChanged(nameof(Cars));
                }
            }
        }
        public Car SelectedCar
        {
            get { return _SelectedCar; }
            set
            {
                if (_SelectedCar != value) // Verifica si el valor ha cambiado
                {
                    _SelectedCar = value;
                    Search(_SelectedCar.Plate);
                    OnPropertyChanged(nameof(SelectedCar));
                }
            }
        }
        public ConcurrentObservableCollection<EntryExitReport> Entries
        {
            get { return _Entries; }
            set
            {
                if (_Entries != value) // Verifica si el valor ha cambiado
                {
                    _Entries = value;
                    OnPropertyChanged(nameof(Entries));
                }
            }
        }
        public ConcurrentObservableCollection<IssueReport> Issues

        {
            get { return _Issues; }
            set
            {
                if (_Issues != value) // Verifica si el valor ha cambiado
                {
                    _Issues = value;
                    OnPropertyChanged(nameof(Issues));
                }
            }
        }
        public ConcurrentObservableCollection<CrashReport> Crashes
        {
            get { return _Crashes; }
            set
            {
                if (_Crashes != value) // Verifica si el valor ha cambiado
                {
                    _Crashes = value;
                    OnPropertyChanged(nameof(Crashes));
                }
            }
        }



        #endregion
        #region Commands
        public RelayCommand<string> ISeeEntryReport { get; set; }
        public RelayCommand<string> ISeeIssueReport { get; set; }
        public RelayCommand<string> ISeeCrashReport { get; set; }
        public RelayCommand<Car> SelectedCarCommand { get; set; }
        public RelayCommand AddCarCommand { get; set; }

        #endregion
        #region Actions

        private async Task AddCar()
        {
            AddCar addCar = new AddCar();
            addCar.ShowDialog();  
        }
        private async Task LoadCars()
        {
            try
            {
                var result = await _aPIService.GetAsync<List<Car>>("api/cars", TimeSpan.FromSeconds(4), useToken: true);
                result = result.OrderBy(e=>e.Model).ToList();
                Cars.AddRange(result);
            }
        
            catch (Exception e)
            {

                throw;
            }
        }

        private async Task LoadCar(Car car)
        {
        
        }

        private async Task Search(string plate)
        {
            SearchCrashesAsync(plate);
            SearchEntriesAsync(plate);
            SearchIssuesAsync(plate);
        }

        private async void SearchEntriesAsync(string Plate)
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

              
                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

              

                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/EntryExitReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                // Llamada al servicio API
                var data = await _aPIService.GetAsync<IEnumerable<EntryExitReport>>(endpoint, TimeSpan.FromSeconds(10));

                Entries = new();
                if( data !=null)
                {
                    Entries.AddRange(data);
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.Error.WriteLine($"Error al buscar entradas: {ex.Message}");
                throw; // Se relanza la excepción para que el llamador pueda manejarla
            }
        }
      
        private async Task SearchIssuesAsync(string Plate)
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

              

                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

              
                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/IssueReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                // Llamada al servicio API
                var data = await _aPIService.GetAsync<IEnumerable<IssueReport>>(endpoint, TimeSpan.FromSeconds(10));

                Issues = new();
                if ( data != null)
                {
                    Issues.AddRange(data);
                }



            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.Error.WriteLine($"Error al buscar entradas: {ex.Message}");
                throw; // Se relanza la excepción para que el llamador pueda manejarla
            }
        }
       
        private async Task SearchCrashesAsync(string Plate)
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

         
                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

          

                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/CrashReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                // Llamada al servicio API
                var data = await _aPIService.GetAsync<IEnumerable<CrashReport>>(endpoint, TimeSpan.FromSeconds(10));

                Crashes = new();
                if ( data != null)
                {
                    Crashes.AddRange(data);
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.Error.WriteLine($"Error al buscar entradas: {ex.Message}");
                throw; // Se relanza la excepción para que el llamador pueda manejarla
            }
        }

        private async void SeeEntry(string id)
        {
            SharedData.EntryExitReport = Entries.FirstOrDefault(e => e.ReportId == id);
            ViewEntry viewEntry = new ViewEntry();
            viewEntry.Show();
        }
      
        private async void Seeissue(string id)
        {
            SharedData.IssueReport = Issues.FirstOrDefault(e => e.ReportId == id);
            ViewIssue viewIssue = new ViewIssue();
            viewIssue.Show();
        }
      
        private async void SeeCrash(string id)
        {
            SharedData.CrashReport = Crashes.FirstOrDefault(e => e.ReportId == id);
            ViewCrash viewCrash = new ViewCrash();
            viewCrash.Show();
        }

        #endregion
    }
}
