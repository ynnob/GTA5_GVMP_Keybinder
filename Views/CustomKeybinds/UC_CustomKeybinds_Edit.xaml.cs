using GTA5_Keybinder_Bonnyfication.ViewModels.CustomKeybinds;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GTA5_Keybinder_Bonnyfication.Views.CustomKeybinds
{
    /// <summary>
    /// Interaction logic for UC_CustormKeybinds_Edit.xaml
    /// </summary>
    public partial class UC_CustomKeybinds_Edit : UserControl
    {
        public UC_CustomKeybinds_Edit()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Send Event to ViewModel
            var vm = (VM_CustomKeybinds)DataContext;
            if (vm.PreviewKeyDown.CanExecute(null))
            {
                vm.PreviewKeyDown.Execute(e);
            }
        }
    }
}
