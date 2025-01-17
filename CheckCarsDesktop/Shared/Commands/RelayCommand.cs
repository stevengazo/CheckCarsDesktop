using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckCarsDesktop.Shared.Commands
{

    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Func<object?, Task>? _executeAsync;
        private readonly Action<object?>? _executeSync;
        private readonly Func<object?, bool> _canExecute;

        // Constructor para comandos síncronos
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _executeSync = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true); // Si no se proporciona, siempre puede ejecutarse
        }

        // Constructor para comandos asíncronos
        public RelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            _executeAsync = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true); // Si no se proporciona, siempre puede ejecutarse
        }

        public bool CanExecute(object? parameter) => _canExecute(parameter);

        public async void Execute(object? parameter)
        {
            // Si el comando es asíncrono, ejecutar como tarea asíncrona
            if (_executeAsync != null)
            {
                await _executeAsync(parameter);
            }
            // Si es un comando síncrono, ejecutar como acción normal
            else if (_executeSync != null)
            {
                _executeSync(parameter);
            }
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}