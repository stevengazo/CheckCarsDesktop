using CheckCarsDesktop.Models;
using CheckCarsDesktop.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.ViewModels
{
   public class ViewEntryVM : ViewModelBase
    {

        private EntryExitReport _report {  get; set; }

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
            Report = SharedData.EntryExitReport;
            
        }
    }
}
