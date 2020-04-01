using GTA5_Keybinder_Bonnyfication.ViewModels.Einstellungen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTA5_Keybinder_Bonnyfication.Views.Einstellungen
{
    /// <summary>
    /// Interaction logic for V_Einstellungen.xaml
    /// </summary>
    public partial class V_Einstellungen : UserControl
    {
        public V_Einstellungen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validiert die Textbox damit nur Zahlen eingegeben werden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Send Event to ViewModel
            var vm = (VM_Einstellungen)DataContext;
            if (vm.PreviewKeyDown.CanExecute(null))
            {
                vm.PreviewKeyDown.Execute(e);
            }
        }
    }
}
