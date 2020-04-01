using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
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

namespace GTA5_Keybinder_Bonnyfication.Views
{
    /// <summary>
    /// Interaction logic for V_Startup.xaml
    /// </summary>
    public partial class V_Startup : Window
    {
        public V_Startup()
        {
            InitializeComponent();

            // Check App is in Admin Mode 
            // Else start as admin cause god damn one click
            CheckAppStartAsAdmin();
        }

        private void CheckAppStartAsAdmin()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            // OH WIR SIND KEIN ADMIN!!!!!
            if (!runAsAdmin)
            {
                ProcessStartInfo info = new ProcessStartInfo(Application.ResourceAssembly.Location);
                info.UseShellExecute = true;
                if (System.Environment.OSVersion.Version.Major >= 6)
                {
                    info.Verb = "runas";
                }

                // Start the new process
                try
                {
                    Process.Start(info);
                }
                catch (Exception)
                {
                    Bindermessage.ShowError("Ups...Ich kann leider ohne Administrative Rechte nicht richtig funktionieren.");
                }

                Application.Current.Shutdown();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        private string m_statusInfo;
        public string statusInfo
        {
            get { return m_statusInfo; }
            set { m_statusInfo = value; OnPropertyChanged("statusInfo"); }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;

            //Events
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            //start worker ##RUN Splashscreen to End
            backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Öffne Hauptmenü
            openMainWindow();

            //Schließe Splashscreen
            closeSplashscreen();
        }


        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i += 2)
            {
                (sender as BackgroundWorker).ReportProgress(i);

                if (i < 20)
                {
                    Thread.Sleep(85);
                    statusInfo = "Initializing Content...";
                }
                if (i > 20 && i < 50)
                {
                    Thread.Sleep(35);
                    statusInfo = "Prepare for awesomeness...";
                }
                if (i > 50 && i < 100)
                {
                    Thread.Sleep(15);
                    statusInfo = "Initializing UI...";
                }
            }

        }


        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgBar.Value = e.ProgressPercentage;

            if (e.ProgressPercentage < 20)
            {
                statsutext.Text = "Initializing Content...";
            }
            if (e.ProgressPercentage > 20 && e.ProgressPercentage < 50)
            {
                statsutext.Text = "Prepare for awesomeness...";
            }
            if (e.ProgressPercentage > 50 && e.ProgressPercentage <= 100)
            {
                statsutext.Text = "Initializing UI...";
            }
        }


        private void Grid_MouseLeftButtonDown_MoveStartupScreen(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void openMainWindow()
        {
            V_MainWindow mainWindow = new V_MainWindow();
            mainWindow.Show();
        }

        private void closeSplashscreen()
        {
            this.Close();
        }
    }
}
