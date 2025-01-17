using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckCarsDesktop.Views.Shared
{
    /// <summary>
    /// Lógica de interacción para InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        public string? InputText { get; private set; }

        public InputDialog(string title, string prompt)
        {
            InitializeComponent();
            this.Title = title;
            //InputTextBox. = prompt; // Si deseas poner un texto predeterminado
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = InputTextBox.Text;
            DialogResult = true;  // Esto cerrará la ventana y devuelve 'true'
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Esto cerrará la ventana y devuelve 'false'
            Close();
        }
    }
}
