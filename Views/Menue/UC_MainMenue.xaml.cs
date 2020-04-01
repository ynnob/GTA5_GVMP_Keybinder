using GTA5_Keybinder_Bonnyfication.ViewModels;
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

namespace GTA5_Keybinder_Bonnyfication.Views.Menue
{
    /// <summary>
    /// Interaction logic for UC_MainMenue.xaml
    /// </summary>
    public partial class UC_MainMenue : UserControl
    {
        public UC_MainMenue()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((VM_MainWindow)this.DataContext).SearchGTAProcess();
        }
    }
}
