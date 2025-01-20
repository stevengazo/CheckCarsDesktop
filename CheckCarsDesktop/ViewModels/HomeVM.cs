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
        }

        #region Commands

        public RelayCommand<string> ISeeEntryReport {  get; set; }

        #endregion

        #region Properties
        private readonly APIService _APIService = new();

        private DateTime _DateToSearch;
        
        private List<EntryExitReport> _Entries = new();
        private List<IssueReport> _Issues = new();
        private List<CrashReport> _Crashes= new();

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

        public List<EntryExitReport> Entries
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
        public List<IssueReport> Issues

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
        public List<CrashReport> Crashes
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
        private async void LoadDefaultEntries()
        {
            try
            {
                _APIService.Token = SharedData.Token;
                var resu = await _APIService.GetAsync<List<EntryExitReport>>("api/EntryExitReports", TimeSpan.FromSeconds(20), true);
                Entries = resu.ToList();
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
                Issues = resu.ToList();
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
                Crashes = resu.ToList();
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
            viewEntry.ShowDialog();
        }

        
        #endregion


    }
}
