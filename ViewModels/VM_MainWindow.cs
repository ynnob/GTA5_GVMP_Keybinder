using Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GTA5_Keybinder_Bonnyfication.ViewModels
{
    public class VM_MainWindow : Base.ViewModelBase
    {
        public VM_MainWindow(MainController Controller)
        {
            // CONTROLLER zuweisen
            if (Controller == null)
            {
                Bindermessage.ShowError("Die Initialisierung ist fehlgeschlagen! Das Programm wird beendet.");
                return;
            }

            _CONTROLLER = Controller;

            // Versionsnummer entnehmen
            //GetCurrentVersion();

            // Add User to Whitelist!
            AddWhitelist();
        }



        private bool _PROCESS_FOUND = false;


        public void SearchGTAProcess()
        {
            if (_PROCESS_FOUND)
            {
                return;
            }
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_PROCESS_FOUND)
            {
                Status_Keybinder = "Running";
                _CONTROLLER.HookKeybindsOnStart();
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_PROCESS_FOUND)
            {
                Process[] processes = Process.GetProcessesByName("GTA5");
                if (processes.Length > 0)
                {
                    // PROZESS GEFUNDEN
                    _PROCESS_FOUND = true;
                }

                Thread.Sleep(1000);
            }

        }

        #region Properties
        public Version AssemblyVersion
        {
            get
            {
                try
                {

                    return ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }
                catch
                {
                    return null;
                }
            }
        }

        private string m_Status_Keybinder = "Inaktiv (GTA 5 Prozess nicht gefunden)";
        public string Status_Keybinder
        {
            get { return m_Status_Keybinder; }
            set { m_Status_Keybinder = value; OnPropertyChanged("Status_Keybinder"); }
        }



        private MainController _CONTROLLER;

        private string m_version;
        public string version
        {
            get { return m_version; }
            set { m_version = value; OnPropertyChanged("version"); }
        }
        #endregion


        #region Private Methoden
        private void GetCurrentVersion()
        {
            try
            {
                version = AssemblyVersion.ToString(4);
            }
            catch
            {
                version = "ERROR";
            }
        }
        #endregion


        #region Delegate Commands
        #region DelegateCommand Command_Button_Keybinds
        private DelegateCommand m_Command_Button_Keybinds;
        public DelegateCommand Command_Button_Keybinds
        {
            get
            {
                if (m_Command_Button_Keybinds == null)
                { m_Command_Button_Keybinds = new DelegateCommand(Execute_Command_Button_Keybinds, CanExecute_Command_Button_Keybinds); }

                return m_Command_Button_Keybinds;
            }
        }
        private void Execute_Command_Button_Keybinds(object Parameter)
        {
            _CONTROLLER.SetzeUC(_CONTROLLER.VCustomKeybinds, _CONTROLLER.VMCustomKeybinds);
        }
        private bool CanExecute_Command_Button_Keybinds(object Parameter)
        {
            return true;
        }
        #endregion



        #region DelegateCommand Command_Button_Not_Implemented
        private DelegateCommand m_Command_Button_Not_Implemented;
        public DelegateCommand Command_Button_Not_Implemented
        {
            get
            {
                if (m_Command_Button_Not_Implemented == null)
                { m_Command_Button_Not_Implemented = new DelegateCommand(Execute_Command_Button_Not_Implemented, CanExecute_Command_Button_Not_Implemented); }

                return m_Command_Button_Not_Implemented;
            }
        }
        private void Execute_Command_Button_Not_Implemented(object Parameter)
        {
            MessageBox.Show("Ups... Baustelle! \n\n--> Comming Soon!","GVMP Keybinder ",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        private bool CanExecute_Command_Button_Not_Implemented(object Parameter)
        {
            return true;
        }
        #endregion



        #region DelegateCommand Command_Button_Feedback
        private DelegateCommand m_Command_Button_Feedback;
        public DelegateCommand Command_Button_Feedback
        {
            get
            {
                if (m_Command_Button_Feedback == null)
                { m_Command_Button_Feedback = new DelegateCommand(Execute_Command_Button_Feedback, CanExecute_Command_Button_Feedback); }

                return m_Command_Button_Feedback;
            }
        }
        private void Execute_Command_Button_Feedback(object Parameter)
        {
            System.Diagnostics.Process.Start("https://www.gvmp.de/forum/thread/1763-release-beta-gvmp-keybinder-by-bonnyfication-gta5-roleplay-server/");
        }
        private bool CanExecute_Command_Button_Feedback(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Whitelist
        private DelegateCommand m_Command_Button_Whitelist;
        public DelegateCommand Command_Button_Whitelist
        {
            get
            {
                if (m_Command_Button_Whitelist == null)
                { m_Command_Button_Whitelist = new DelegateCommand(Execute_Command_Button_Whitelist, CanExecute_Command_Button_Whitelist); }

                return m_Command_Button_Whitelist;
            }
        }
        private void Execute_Command_Button_Whitelist(object Parameter)
        {
            AddWhitelist(true);
        }
        private bool CanExecute_Command_Button_Whitelist(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Einstellungen
        private DelegateCommand m_Command_Button_Einstellungen;
        public DelegateCommand Command_Button_Einstellungen
        {
            get
            {
                if (m_Command_Button_Einstellungen == null)
                { m_Command_Button_Einstellungen = new DelegateCommand(Execute_Command_Button_Einstellungen, CanExecute_Command_Button_Einstellungen); }

                return m_Command_Button_Einstellungen;
            }
        }
        private void Execute_Command_Button_Einstellungen(object Parameter)
        {
            _CONTROLLER.SetzeUC(_CONTROLLER.VEinstellungen, _CONTROLLER.VMEinstellungen);
        }
        private bool CanExecute_Command_Button_Einstellungen(object Parameter)
        {
            return true;
        }
        #endregion


        #endregion


        #region Private Methoden
        private void AddWhitelist(bool WithMessage = false)
        {
            try
            {
                //http://game.gvmp.de/whitelist.php
                var data = new WebClient().DownloadString("http://server.gvmp.de/gv_whitelist.php?server=1");
                var data2 = new WebClient().DownloadString("http://server.gvmp.de/gv_whitelist.php?server=2");
                var data3 = new WebClient().DownloadString("http://server.gvmp.de/gv_whitelist.php?server=3");

                if (WithMessage)
                {
                    Bindermessage.ShowInfo("Du wurdest der Whitelist hinzugefügt!");
                }
            }
            catch (Exception)
            {
                Bindermessage.ShowError("Die Verbindung zu GVMP ist fehlgeschlagen! \nDu konntest leider nicht automatisch der Whitelist hinzugefügt werden!");
            }

        }
        #endregion
    }
}
