using CheckCarsDesktop.Models;
using CheckCarsDesktop.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckCarsDesktop.Shared.Data;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using CheckCarsDesktop.Views;
using Meziantou.Framework.WPF.Collections;

namespace CheckCarsDesktop.ViewModels
{
    public class HomeVM : ViewModelBase
    {
        public HomeVM()
        {
            Task.Run(() => LoadDefaultEntries());
            Task.Run(() => LoadDefaultIssues());
            Task.Run(() => LoadDefaultCrashes());
            ISeeEntryReport = new RelayCommand<string>(e => SeeEntry(e));
            ISeeCrashReport = new RelayCommand<string>(e => SeeCrash(e));
            ISeeIssueReport = new RelayCommand<string>(e => Seeissue(e));
            ISeeCarsReport = new RelayCommand(() => CarsList());
            ISearch = new RelayCommand(() => SearchAsync());
            ICleanCommand = new RelayCommand(async () => await CleanInputsAsync());
        }



        #region Commands
        public RelayCommand ISeeCarsReport { get; set; }
        public RelayCommand<string> ISeeEntryReport { get; set; }
        public RelayCommand<string> ISeeIssueReport { get; set; }
        public RelayCommand<string> ISeeCrashReport { get; set; }
        public RelayCommand ISearch { get; set; }
        public RelayCommand ICleanCommand { get; set; }
        #endregion

        #region Properties
        private readonly APIService _APIService = new();

        private DateTime _DateToSearch = DateTime.MinValue;
        private string _plate;
        private string _AutoToSearch;


        private ConcurrentObservableCollection<EntryExitReport> _Entries = new();
        private ConcurrentObservableCollection<IssueReport> _Issues = new();
        private ConcurrentObservableCollection<CrashReport> _Crashes = new();

        public DateTime DateToSearch
        {
            get { return _DateToSearch; }
            set
            {
                if (_DateToSearch != value) // Verifica si el valor ha cambiado
                {
                    _DateToSearch = value;
                    OnPropertyChanged(nameof(DateToSearch));
                }
            }
        }
        public string Plate
        {
            get { return _plate; }
            set
            {
                if (_plate != value) // Verifica si el valor ha cambiado
                {
                    _plate = value;
                    OnPropertyChanged(nameof(Plate));
                }
            }
        }
        public string AuthorToSearch
        {
            get { return _AutoToSearch; }
            set
            {
                if (_AutoToSearch != value) // Verifica si el valor ha cambiado
                {
                    _AutoToSearch = value;
                    OnPropertyChanged(nameof(_AutoToSearch));
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


        #region Methods
        private void CarsList()
        {
            CarList carList = new CarList();
            carList.ShowDialog();
        }
        private async Task CleanInputsAsync()
        {
            DateToSearch = DateTime.MinValue;
            Plate = string.Empty;
            AuthorToSearch = string.Empty;
            loadDefault();
            await Task.CompletedTask;
        }
        private async Task loadDefault()
        {

            LoadDefaultEntries();
            LoadDefaultIssues();
            LoadDefaultCrashes();
            await Task.CompletedTask;
        }
        private async void LoadDefaultEntries()
        {
            try
            {
                _APIService.Token = SharedData.Token;
                var resu = await _APIService.GetAsync<List<EntryExitReport>>("api/EntryExitReports", TimeSpan.FromSeconds(20), true);
                foreach (var item in resu)
                {
                    Entries.Add(item);
                }
            }
            catch (Exception e)
            {

                //    throw;
            }
        }
        private async void LoadDefaultIssues()
        {
            try
            {
                _APIService.Token = SharedData.Token;
                var resu = await _APIService.GetAsync<List<IssueReport>>("api/IssueReports", TimeSpan.FromSeconds(20), true);
                foreach (var item in resu)
                {
                    Issues.Add(item);
                }
            }
            catch (Exception e)
            {

                //    throw;
            }
        }
        private async void LoadDefaultCrashes()
        {
            try
            {

                _APIService.Token = SharedData.Token;
                var resu = await _APIService.GetAsync<List<CrashReport>>("api/CrashReports", TimeSpan.FromSeconds(20), true);
                foreach (var item in resu)
                {
                    Crashes.Add(item);
                }
            }
            catch (Exception e)
            {

                //    throw;
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
        private void SearchAsync()
        {
            SearchEntriesAsync();
            SearchIssuesAsync();
            SearchCrashesAsync();
            Task.WaitAll();
        }

        private async void SearchEntriesAsync()
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

                if (DateToSearch != DateTime.MinValue)
                {
                    queryParameters.Add($"date={DateToSearch:yyyy-MM-dd}");
                }

                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

                if (!string.IsNullOrEmpty(AuthorToSearch))
                {
                    queryParameters.Add($"author={Uri.EscapeDataString(AuthorToSearch)}");
                }

                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/EntryExitReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                if (queryParameters.Count != 0)
                {

                    // Llamada al servicio API
                    var data = await _APIService.GetAsync<IEnumerable<EntryExitReport>>(endpoint, TimeSpan.FromSeconds(10));

                    Entries = new();
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
        private async Task SearchIssuesAsync()
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

                if (DateToSearch != DateTime.MinValue)
                {
                    queryParameters.Add($"date={DateToSearch:yyyy-MM-dd}");
                }

                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

                if (!string.IsNullOrEmpty(AuthorToSearch))
                {
                    queryParameters.Add($"author={Uri.EscapeDataString(AuthorToSearch)}");
                }

                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/IssueReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                // Llamada al servicio API
                var data = await _APIService.GetAsync<IEnumerable<IssueReport>>(endpoint, TimeSpan.FromSeconds(10));

                Issues = new();
                Issues.AddRange(data);


            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.Error.WriteLine($"Error al buscar entradas: {ex.Message}");
                throw; // Se relanza la excepción para que el llamador pueda manejarla
            }
        }
        private async Task SearchCrashesAsync()
        {
            try
            {
                // Construcción de los parámetros de consulta
                var queryParameters = new List<string>();

                if (DateToSearch != DateTime.MinValue)
                {
                    queryParameters.Add($"date={DateToSearch:yyyy-MM-dd}");
                }

                if (!string.IsNullOrEmpty(Plate))
                {
                    queryParameters.Add($"plate={Uri.EscapeDataString(Plate)}");
                }

                if (!string.IsNullOrEmpty(AuthorToSearch))
                {
                    queryParameters.Add($"author={Uri.EscapeDataString(AuthorToSearch)}");
                }

                // Construcción del endpoint con parámetros de consulta
                var endpoint = "api/CrashReports/search";
                if (queryParameters.Any())
                {
                    endpoint = $"{endpoint}?{string.Join("&", queryParameters)}";
                }

                // Llamada al servicio API
                var data = await _APIService.GetAsync<IEnumerable<CrashReport>>(endpoint, TimeSpan.FromSeconds(10));

                Crashes = new();
                Crashes.AddRange(data);


            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.Error.WriteLine($"Error al buscar entradas: {ex.Message}");
                throw; // Se relanza la excepción para que el llamador pueda manejarla
            }
        }

        #endregion


    }
}
