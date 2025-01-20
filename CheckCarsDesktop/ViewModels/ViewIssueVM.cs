using CheckCarsDesktop.Models;
using CheckCarsDesktop.Services;
using CheckCarsDesktop.Shared.Data;
using GMap.NET.MapProviders;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.ViewModels
{
    public class ViewIssueVM : ViewModelBase
    {

        private readonly APIService _APIService = new();

        private ObservableCollection<Photo> photos = new();
        private PointLatLng _ubication { get; set; }
        private IssueReport _report { get; set; }
        public OpenStreetMapProvider MapProvider { get; set; } = OpenStreetMapProvider.Instance;
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
        public IssueReport Report
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
        public PointLatLng Ubication
        {
            get { return _ubication; }
            set
            {
                if (_ubication != value) // Verifica si el valor ha cambiado
                {
                    _ubication = value;
                    OnPropertyChanged(nameof(Ubication));
                }
            }
        }
        public ViewIssueVM()
        {
            _APIService.Token = SharedData.Token;
            Report = SharedData.IssueReport;
            LoadImgs();
            LoadUbication();
        }
        private void LoadUbication()
        {
            if (Report != null)
            {
                Ubication = new PointLatLng(Report.Latitude, Report.Longitude);
            }
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
