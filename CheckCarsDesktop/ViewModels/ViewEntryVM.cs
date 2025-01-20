using CheckCarsDesktop.Models;
using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.ViewModels
{
    public class ViewEntryVM : ViewModelBase
    {
        private readonly APIService _APIService = new();
        private ObservableCollection<Photo> photos = new();
        private EntryExitReport _report { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return photos; }
            set
            {
                if (photos != value) // Verifica si el valor ha cambiado
                {
                    photos = value;
                    OnPropertyChanged(nameof(Photos));
                }
            }
        }
        public EntryExitReport Report
        {
            get { return _report; }
            set
            {
                if (_report != value) // Verifica si el valor ha cambiado
                {
                    _report = value;
                    OnPropertyChanged(nameof(Report));
                }
            }
        }

        public ViewEntryVM()
        {
            _APIService.Token = SharedData.Token;
            Report = SharedData.EntryExitReport;
            LoadImgs();
        }

        private async void LoadImgs()
        {
            if (Report != null)
            {
                Report.Photos = await _APIService.GetAsync<List<Photo>>($"/api/Photos/report/{Report.ReportId}", TimeSpan.FromSeconds(20), true);

                if (Report.Photos != null)
                {
                    foreach (Photo photo in Report.Photos)
                    {
                        Photos.Add(photo);
                    }
                }
            }
        }
    }
}
