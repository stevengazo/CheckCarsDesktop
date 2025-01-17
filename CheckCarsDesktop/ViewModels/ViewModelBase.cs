using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.ViewModels
{
   public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Evento para notificar cambios en las propiedades.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Método para notificar cambios en una propiedad específica.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad que cambió.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Establece un valor en una propiedad y notifica el cambio si el valor es diferente.
        /// </summary>
        /// <typeparam name="T">Tipo de la propiedad.</typeparam>
        /// <param name="field">Referencia al campo backing de la propiedad.</param>
        /// <param name="value">Nuevo valor a asignar.</param>
        /// <param name="propertyName">Nombre de la propiedad (opcional).</param>
        /// <returns>True si el valor fue cambiado, False en caso contrario.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}