using Base;
using GTA5_Keybinder_Bonnyfication.Models;
using GTA5_Keybinder_Bonnyfication.ViewModels.CustomKeybinds;
using GTA5_Keybinder_Bonnyfication.Views;
using GTA5_Keybinder_Bonnyfication.Views.CustomKeybinds;
using GTA5_Keybinder_Bonnyfication.Views.Menue;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Windows.Interop;
using System.Windows;
using AutoHotkey.Interop;
using GTA5_Keybinder_Bonnyfication.ViewModels.Einstellungen;
using GTA5_Keybinder_Bonnyfication.Views.Einstellungen;

namespace GTA5_Keybinder_Bonnyfication.ViewModels
{
    /// <summary>
    /// Main Handler der Controls und Keybinds
    /// GLOBAL CATCHER
    /// </summary>
    public class MainController : Base.ViewModelBase
    {

        public MainController(V_MainWindow Main)
        {
            _MAIN = Main;

            // Set Window Resolution Properties
            setWindowResolution();

            // Open Main Window ~MainMenue
            InitMainMenue();

            // Einstellungen aus Dateien Laden
            LoadKeybinds();
            LoadUserSettings();
            //HookKeybindsOnStart();
        }





        #region GLOBAL
        GTA5WindowHandle WindowHandle = new GTA5WindowHandle();
        AutoHotkeyEngine _AHK = AutoHotkeyEngine.Instance;
        V_MainWindow _MAIN;
        #endregion

        #region Properties
        private KeybindToAHK AHKHandler = new KeybindToAHK();

        private UserControl m_ContentController;
        public UserControl ContentController
        {
            get { return m_ContentController; }
            set { m_ContentController = value; OnPropertyChanged("ContentController"); }
        }

        #region MainMenue
        private VM_MainWindow m_VMMainWindow;
        public VM_MainWindow VMMainWindow
        {
            get
            {
                if (m_VMMainWindow == null)
                {
                    m_VMMainWindow = new VM_MainWindow(this);
                }
                return m_VMMainWindow;
            }
            set { m_VMMainWindow = value; OnPropertyChanged("VMMainWindow"); }
        }
        private UserControl m_VMainWindow;
        public UserControl VMainWindow
        {
            get
            {
                if (m_VMainWindow == null)
                {
                    m_VMainWindow = new UC_MainMenue();
                }
                return m_VMainWindow;
            }
            set { m_VMainWindow = value; OnPropertyChanged("VMainWindow"); }
        }
        #endregion

        #region Keybinds
        private VM_CustomKeybinds m_VMCustomKeybinds;
        public VM_CustomKeybinds VMCustomKeybinds
        {
            get
            {
                if (m_VMCustomKeybinds == null)
                {
                    m_VMCustomKeybinds = new VM_CustomKeybinds(this);
                }
                return m_VMCustomKeybinds;
            }
            set { m_VMCustomKeybinds = value; OnPropertyChanged("VMCustomKeybinds"); }
        }

        private UserControl m_VCustomKeybinds;
        public UserControl VCustomKeybinds
        {
            get
            {
                if (m_VCustomKeybinds == null)
                {
                    m_VCustomKeybinds = new UC_CustomKeybinds();
                }
                return m_VCustomKeybinds;
            }
            set { m_VCustomKeybinds = value; OnPropertyChanged("VCustomKeybinds"); }
        }

        private UserControl m_VCustomKeybindsEdit;
        public UserControl VCustomKeybindsEdit
        {
            get
            {
                if (m_VCustomKeybindsEdit == null)
                {
                    m_VCustomKeybindsEdit = new UC_CustomKeybinds_Edit();
                }
                return m_VCustomKeybindsEdit;
            }
            set { m_VCustomKeybindsEdit = value; OnPropertyChanged("VCustomKeybindsEdit"); }
        }

        #endregion

        #region Einstellungen
        private VM_Einstellungen m_VMEinstellungen;
        public VM_Einstellungen VMEinstellungen
        {
            get
            {
                if (m_VMEinstellungen == null)
                {
                    m_VMEinstellungen = new VM_Einstellungen(this);
                }
                return m_VMEinstellungen;
            }
            set { m_VMEinstellungen = value; }
        }

        private UserControl m_VEinstellungen;
        public UserControl VEinstellungen
        {
            get
            {
                if (m_VEinstellungen == null)
                {
                    m_VEinstellungen = new V_Einstellungen();
                }
                return m_VEinstellungen;
            }
            set { m_VEinstellungen = value; OnPropertyChanged("VEinstellungen"); }
        }


        #endregion

        private M_Settings m_Aktuelle_Einstellungen;
        public M_Settings Aktuelle_Einstellungen
        {
            get { return m_Aktuelle_Einstellungen; }
            set { m_Aktuelle_Einstellungen = value; OnPropertyChanged("Aktuelle_Einstellungen"); }
        }


        private ObservableCollection<M_Binding> m_Auflistung_Keybinds;
        public ObservableCollection<M_Binding> Auflistung_Keybinds
        {
            get
            {
                if (m_Auflistung_Keybinds == null)
                {
                    m_Auflistung_Keybinds = new ObservableCollection<M_Binding>();
                }
                return m_Auflistung_Keybinds;
            }
            set { m_Auflistung_Keybinds = value; OnPropertyChanged("Auflistung_Keybinds"); }
        }


        private double m_breite = 1080.00;
        public double breite
        {
            get { return m_breite; }
            set { m_breite = value; OnPropertyChanged("breite"); }
        }

        private double m_hoehe = 720.00;
        public double hoehe
        {
            get { return m_hoehe; }
            set { m_hoehe = value; OnPropertyChanged("hoehe"); }
        }

        private double m_minbreite = 1080.00;
        public double minbreite
        {
            get { return m_minbreite; }
            set { m_minbreite = value; OnPropertyChanged("minbreite"); }
        }

        private double m_minhoehe = 720.00;
        public double minhoehe
        {
            get { return m_minhoehe; }
            set { m_minhoehe = value; OnPropertyChanged("minhoehe"); }
        }
        #endregion

        #region Öffentliche Methoden
        public void SetzeUC(UserControl uc, object vm)
        {
            ContentController = uc;
            ContentController.DataContext = vm;
        }

        public void RecreateAHKFile()
        {
            // AHk File neu schreiben
            _AHK.Terminate();
            StoreKeybinds();
            RestartKeybinder();
        }

        public void RestartKeybinder()
        {
            Bindermessage.ShowInfo("Der Keybinder wird neu gestartet, um die Änderungen zu übernehmen!");

            ProcessStartInfo info = new ProcessStartInfo(Application.ResourceAssembly.Location);
            info.UseShellExecute = true;
            if (System.Environment.OSVersion.Version.Major >= 6)
            {
                info.Verb = "runas";
            }

            try
            {
                Process.Start(info);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Bindermessage.ShowError("Beim Versuch den Keybinder als Administrator zu starten, ist ein Fehler aufgetreten! \n\n" + ex.Message);
            }
            

        }

        public void LoadAHKScript()
        {
            // Directory
            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";
            string m_FileName = "Keybinds_Script.ahk";
            string m_Fullpath = Path.Combine(m_appdatapath, m_foldername, m_FileName);

            //_AHK.Reset();

            _AHK.LoadFile(m_Fullpath);            
        }
        #endregion

        #region Private Methoden
        private void InitMainMenue()
        {
            SetzeUC(VMainWindow, VMMainWindow);
        }

        private void setWindowResolution()
        {
            breite = 1080.00;
            hoehe = 720.00;
            minbreite = 1080.00;
            minhoehe = 720.00;
        }

        private void StoreKeybinds()
        {
            if (m_Auflistung_Keybinds.Count == 0)
            {
                return;
            }

            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";
            string m_FileName = "Keybinds_Store_Extended.xml";
            string m_Fullpath = Path.Combine(m_appdatapath, m_foldername, m_FileName);

            try
            {
                XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<M_Binding>));
                using (StreamWriter writ = new StreamWriter(m_Fullpath))
                {
                    seri.Serialize(writ, m_Auflistung_Keybinds);
                }
            }
            catch (Exception ex)
            {
                Bindermessage.ShowError("Fehler beim Speichern der Keybinds!\n\nError:\n" + ex);
            }
        }

        private void LoadKeybinds()
        {
            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";

            string m_FileName_Old = "Keybinds_Store.xml";
            string m_FileNameExtended = "Keybinds_Store_Extended.xml";

            string m_FullpathOld = Path.Combine(m_appdatapath, m_foldername, m_FileName_Old);
            string m_FullpathExtended = Path.Combine(m_appdatapath, m_foldername, m_FileNameExtended);

            string m_FullFolderPath = Path.Combine(m_appdatapath, m_foldername);


            // C:\Users\Bonnyfication\AppData\Roaming\Bonnyfication_Keybinds.xml
            //Check Folder exists or Create it
            System.IO.Directory.CreateDirectory(m_FullFolderPath);

            // Wenn die alte Keybinder XML besteht auswerten, Convertieren und Löschen
            if (File.Exists(m_FullpathOld))
            {
                //CONVERT AND DELETE
                ObservableCollection<M_Keybind> m_tmp = new ObservableCollection<M_Keybind>();

                try
                {
                    XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<M_Keybind>));
                    using (StreamReader read = new StreamReader(m_FullpathOld))
                    {
                        m_tmp = seri.Deserialize(read) as ObservableCollection<M_Keybind>;
                    }
                }
                catch (Exception ex)
                {
                    Bindermessage.ShowError("Fehler beim Laden der Keybinds!\n\nError:\n" + ex);
                }


                if (m_tmp.Count > 0)
                {
                    m_Auflistung_Keybinds = new ObservableCollection<M_Binding>();

                    foreach (M_Keybind item in m_tmp)
                    {
                        M_Binding bin = new M_Binding();
                        bin.Bezeichnung = item.Bezeichnung;
                        bin.KeyValue = item.KeyValue;
                        bin.KeyValueForms = item.KeyValueForms;
                        bin.ModValue = item.ModValue;
                        bin.AutoEnter = true;

                        M_BindingOption opt = new M_BindingOption();
                        opt.cmd = item.cmd;

                        bin.Auflistung_BindingOptions = new List<M_BindingOption>();
                        bin.Auflistung_BindingOptions.Add(opt);
                        m_Auflistung_Keybinds.Add(bin);
                    }

                    // Speichere die Bindings im neuen Format

                    try
                    {
                        XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<M_Binding>));
                        using (StreamWriter writ = new StreamWriter(m_FullpathExtended))
                        {
                            seri.Serialize(writ, m_Auflistung_Keybinds);
                        }
                    }
                    catch (Exception ex)
                    {
                        Bindermessage.ShowError("Fehler beim Speichern der konvertierten Keybinds!\n\nError:\n" + ex);
                        Application.Current.Shutdown();
                    }

                    OnPropertyChanged("Auflistung_Keybinds");

                    try
                    {
                        File.Delete(m_FullpathOld);
                    }
                    catch (Exception ex)
                    {
                        Bindermessage.ShowError("Die Datei Keybinder_Store.xml konnte nicht gelöscht werden. Dies ist allerdings erforderlich, damit beim start des Keybinders neue Keybinds nicht überschrieben werden.\nSie können die Datei manuell löschen!\n\nPfad:\n" + m_FullpathOld + "\n\nError:" + ex.Message);
                        Application.Current.Shutdown();
                    }
                }

            }
            else // Existiert die Aktuelle Keybinder_Store_Extended.xml ?
            {
                if (!File.Exists(m_FullpathExtended))
                { return; }

                // Auslesen
                Auflistung_Keybinds = new ObservableCollection<M_Binding>();

                try
                {
                    XmlSerializer seri = new XmlSerializer(typeof(ObservableCollection<M_Binding>));
                    using (StreamReader read = new StreamReader(m_FullpathExtended))
                    {
                        m_Auflistung_Keybinds = seri.Deserialize(read) as ObservableCollection<M_Binding>;
                    }
                }
                catch (Exception ex)
                {
                    Bindermessage.ShowError("Fehler beim Laden der Keybinds!\n\nError:\n" + ex);
                }

                OnPropertyChanged("Auflistung_Keybinds");
            }
        }

        private void LoadUserSettings()
        {
            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";
            string m_FileName = "Keybinds_Settings.xml";
            string m_Fullpath = Path.Combine(m_appdatapath, m_foldername, m_FileName);
            string m_FullFolderPath = Path.Combine(m_appdatapath, m_foldername);

            //Check File exists
            System.IO.Directory.CreateDirectory(m_FullFolderPath);
            if (!File.Exists(m_Fullpath))
            {
                // Wenn Datei nicht existiert Standardwerte setzen
                Aktuelle_Einstellungen = new M_Settings();
                Aktuelle_Einstellungen.Delay = 60;
                Aktuelle_Einstellungen.MultiCommanDelay = 1000;
                Aktuelle_Einstellungen.Name = "Spieler_Name";
                // Speichern
                try
                {
                    XmlSerializer seril = new XmlSerializer(typeof(M_Settings));
                    using (StreamWriter writ = new StreamWriter(m_Fullpath))
                    {
                        seril.Serialize(writ, Aktuelle_Einstellungen);
                    }
                }
                catch (Exception ex)
                {
                    Bindermessage.ShowError("Fehler beim Anlegen der Default Einstellungen!\n\nError:\n" + ex);
                }

                return;
            }

            Aktuelle_Einstellungen = new M_Settings();

            try
            {
                XmlSerializer seri = new XmlSerializer(typeof(M_Settings));
                using (StreamReader read = new StreamReader(m_Fullpath))
                {
                    Aktuelle_Einstellungen = seri.Deserialize(read) as M_Settings;
                }
            }
            catch (Exception ex)
            {
                Bindermessage.ShowError("Fehler beim Laden der Einstellungen!\n\nError:\n" + ex);
            }

        }

        public void HookKeybindsOnStart()
        {
            if (Auflistung_Keybinds.Count == 0)
            {
                return;
            }

            // AHk File neu schreiben
            AHKHandler.CreateAHKFile(m_Auflistung_Keybinds, m_Aktuelle_Einstellungen);
            
            LoadAHKScript();
        }

        #endregion
    }
}