using Base;
using GTA5_Keybinder_Bonnyfication.Enums;
using GTA5_Keybinder_Bonnyfication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace GTA5_Keybinder_Bonnyfication.ViewModels.CustomKeybinds
{
    public class VM_CustomKeybinds : Base.ViewModelBase
    {
        public VM_CustomKeybinds(MainController controller)
        {
            // CONTROLLER zuweisen
            if (controller == null)
            {
                Bindermessage.ShowError("Die Initialisierung ist fehlgeschlagen! Das Programm wird beendet.");
                return;
            }

            _CONTROLLER = controller;

            LoadKeybinds();
        }

        #region Private Methoden
        private void LoadKeybinds()
        {
            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";
            string m_FileNameExtended = "Keybinds_Store_Extended.xml";
            string m_FullpathExtended = Path.Combine(m_appdatapath, m_foldername, m_FileNameExtended);
            string m_FullFolderPath = Path.Combine(m_appdatapath, m_foldername);


            // C:\Users\Bonnyfication\AppData\Roaming\Bonnyfication_Keybinds.xml
            //Check Folder exists or Create it
            System.IO.Directory.CreateDirectory(m_FullFolderPath);

            // Auslesen
            Auflistung_Keybinds = new ObservableCollection<M_Binding>();

            if (!File.Exists(m_FullpathExtended))
            { return; }

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
        #endregion


        #region Properties
        private ObservableCollection<M_Binding> m_Auflistung_Keybinds;
        public ObservableCollection<M_Binding> Auflistung_Keybinds
        {
            get { return m_Auflistung_Keybinds; }
            set { m_Auflistung_Keybinds = value; OnPropertyChanged("Auflistung_Keybinds"); }
        }

        private string m_Hotkey = String.Empty;
        public string Hotkey
        {
            get { return m_Hotkey; }
            set { m_Hotkey = value; OnPropertyChanged("Hotkey"); }
        }

        private string m_Command = String.Empty;
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; OnPropertyChanged("Command"); }
        }

        private bool m_AutoEnter = true;
        public bool AutoEnter
        {
            get { return m_AutoEnter; }
            set { m_AutoEnter = value; OnPropertyChanged("AutoEnter"); }
        }



        private string m_Hotkey_Edit = String.Empty;
        public string Hotkey_Edit
        {
            get { return m_Hotkey_Edit; }
            set { m_Hotkey_Edit = value; OnPropertyChanged("Hotkey_Edit"); }
        }

        private string m_Command_Edit = String.Empty;
        public string Command_Edit
        {
            get { return m_Command_Edit; }
            set { m_Command_Edit = value; OnPropertyChanged("Command_Edit"); }
        }

        private bool m_AutoEnter_Edit;
        public bool AutoEnter_Edit
        {
            get { return m_AutoEnter_Edit; }
            set { m_AutoEnter_Edit = value; OnPropertyChanged("AutoEnter_Edit"); }
        }

        private bool isEditMode = false;

        private int m_CurrentKeyValue;
        public int CurrentKeyValue
        {
            get { return m_CurrentKeyValue; }
            set { m_CurrentKeyValue = value; }
        }

        private int m_CurrentKeyModValue;
        public int CurrentKeyModValue
        {
            get { return m_CurrentKeyModValue; }
            set { m_CurrentKeyModValue = value; }
        }

        private M_Binding m_Altes_Binding = null;

        private MainController _CONTROLLER;

        #endregion

        #region Commands
        #region DelegateCommand Command_Button_Back
        private DelegateCommand m_Command_Button_Back;
        public DelegateCommand Command_Button_Back
        {
            get
            {
                if (m_Command_Button_Back == null)
                { m_Command_Button_Back = new DelegateCommand(Execute_Command_Button_Back, CanExecute_Command_Button_Back); }

                return m_Command_Button_Back;
            }
        }
        private void Execute_Command_Button_Back(object Parameter)
        {
            _CONTROLLER.SetzeUC(_CONTROLLER.VMainWindow, _CONTROLLER.VMMainWindow);
        }
        private bool CanExecute_Command_Button_Back(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Delete_Hotkey
        private DelegateCommand m_Command_Button_Delete_Hotkey;
        public DelegateCommand Command_Button_Delete_Hotkey
        {
            get
            {
                if (m_Command_Button_Delete_Hotkey == null)
                { m_Command_Button_Delete_Hotkey = new DelegateCommand(Execute_Command_Button_Delete_Hotkey, CanExecute_Command_Button_Delete_Hotkey); }

                return m_Command_Button_Delete_Hotkey;
            }
        }
        private void Execute_Command_Button_Delete_Hotkey(object Parameter)
        {
            // Parameter Korrekt?
            if (Parameter == null)
            {
                return;
            }

            if ((M_Binding)Parameter == null)
            {
                return;
            }

            m_Auflistung_Keybinds.Remove((M_Binding)Parameter);
            OnPropertyChanged("Auflistung_Keybinds");
        }
        private bool CanExecute_Command_Button_Delete_Hotkey(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Add_Hotkey
        private DelegateCommand m_Command_Button_Add_Hotkey;
        public DelegateCommand Command_Button_Add_Hotkey
        {
            get
            {
                if (m_Command_Button_Add_Hotkey == null)
                { m_Command_Button_Add_Hotkey = new DelegateCommand(Execute_Command_Button_Add_Hotkey, CanExecute_Command_Button_Add_Hotkey); }

                return m_Command_Button_Add_Hotkey;
            }
        }
        private void Execute_Command_Button_Add_Hotkey(object Parameter)
        {
            if (m_Command == null || m_Hotkey == null)
            {
                return;
            }

            if (m_Hotkey.Trim().Length != 0 && m_Hotkey != "BELEGT" && m_Hotkey != "ERROR" && m_Command.Trim().Length != 0)
            {
                // Baue Hotkey und Hotkey Display Objekt
                M_Binding Disp = new M_Binding(); 
                Disp.Bezeichnung = m_Hotkey;
                Disp.KeyValue = CurrentKeyValue;
                Disp.KeyValueForms = (int)(System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey((Key)CurrentKeyValue);
                Disp.ModValue = CurrentKeyModValue;
                Disp.AutoEnter = AutoEnter;

                Disp.Auflistung_BindingOptions = new List<M_BindingOption>();

                // Command aufsplitten, wenn Contains ~
                if (m_Command.Contains("~"))
                {
                    if (!AutoEnter)
                    {
                        // Hinweis Anzeigen, wenn Mehrere Commands da sind, aber AutoEnter False ist
                        if (MessageBoxResult.No == Bindermessage.ShowQuestionYesNo("Sie haben den Hotkey mit mehreren Befehlen belegt und die Auto-Enter Funktion deaktiviert!\nBeachten Sie, dass hierdurch nur der letzte Befehl nicht automatisch abgesendet wird!\n\nMöchten Sie den Hotkey trotzdem hinzufügen?"))
                        {
                            return;
                        }
                    }


                    string[] commands = m_Command.Split('~');

                    foreach (string command in commands)
                    {
                        M_BindingOption opt = new M_BindingOption();
                        opt.cmd = command.TrimStart();
                        Disp.Auflistung_BindingOptions.Add(opt);
                    }
                }
                else
                {
                    M_BindingOption opt = new M_BindingOption();
                    opt.cmd = m_Command.TrimStart();
                    Disp.Auflistung_BindingOptions.Add(opt);
                }

                m_Auflistung_Keybinds.Add(Disp);
                OnPropertyChanged("Auflistung_Keybinds");


                // RESET FIELDS
                m_Hotkey = String.Empty;
                m_Command = String.Empty;

                OnPropertyChanged("Hotkey");
                OnPropertyChanged("Command");
            }
        }
        private bool CanExecute_Command_Button_Add_Hotkey(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Save
        private DelegateCommand m_Command_Button_Save;
        public DelegateCommand Command_Button_Save
        {
            get
            {
                if (m_Command_Button_Save == null)
                { m_Command_Button_Save = new DelegateCommand(Execute_Command_Button_Save, CanExecute_Command_Button_Save); }

                return m_Command_Button_Save;
            }
        }
        private void Execute_Command_Button_Save(object Parameter)
        {
            _CONTROLLER.Auflistung_Keybinds = new ObservableCollection<M_Binding>(m_Auflistung_Keybinds);
            _CONTROLLER.RecreateAHKFile();

        }
        private bool CanExecute_Command_Button_Save(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Cancel
        private DelegateCommand m_Command_Button_Cancel;
        public DelegateCommand Command_Button_Cancel
        {
            get
            {
                if (m_Command_Button_Cancel == null)
                { m_Command_Button_Cancel = new DelegateCommand(Execute_Command_Button_Cancel, CanExecute_Command_Button_Cancel); }

                return m_Command_Button_Cancel;
            }
        }
        private void Execute_Command_Button_Cancel(object Parameter)
        {
            m_Auflistung_Keybinds = new ObservableCollection<M_Binding>(_CONTROLLER.Auflistung_Keybinds);
            OnPropertyChanged("Auflistung_Keybinds");

            _CONTROLLER.SetzeUC(_CONTROLLER.VMainWindow, _CONTROLLER.VMMainWindow);
        }
        private bool CanExecute_Command_Button_Cancel(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand PreviewKeyDown
        private DelegateCommand m_PreviewKeyDown;
        public DelegateCommand PreviewKeyDown
        {
            get
            {
                if (m_PreviewKeyDown == null)
                { m_PreviewKeyDown = new DelegateCommand(Execute_PreviewKeyDown, CanExecute_PreviewKeyDown); }

                return m_PreviewKeyDown;
            }
        }
        private void Execute_PreviewKeyDown(object Parameter)
        {
            // Ket the Key User pressed
            // Grap input
            CurrentKeyModValue = 0;

            KeyEventArgs e = (KeyEventArgs)Parameter;
            e.Handled = true;

            // Fetch shortcut key
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);

            // Ignore modifier keys.
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }

            // Check Valid Key?
            bool m_valid = false;
            foreach (AHKKey ahkkey in Enum.GetValues(typeof(AHKKey)))
            {
                // Suche gültigen AHK Key
                if ((int)ahkkey == (int)key)
                {
                    m_valid = true;
                    break;
                }
            }

            if (m_valid == false)
            {
                if (!isEditMode)
                {
                    Hotkey = "ERROR";
                }
                else
                {
                    Hotkey_Edit = "ERROR";
                }
                return;
            }


            // Build the shortcut key name.
            StringBuilder shortcutText = new StringBuilder();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                shortcutText.Append("Ctrl+");
                CurrentKeyModValue = (int)ModifierKeys.Control;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                shortcutText.Append("Shift+");
                CurrentKeyModValue = (int)ModifierKeys.Shift;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                shortcutText.Append("Alt+");
                CurrentKeyModValue = (int)ModifierKeys.Alt;
            }
            shortcutText.Append(key.ToString());


            if (!isEditMode)
            {
                foreach (M_Binding item in m_Auflistung_Keybinds)
                {
                    if (item.Bezeichnung == shortcutText.ToString())
                    {
                        Hotkey = "BELEGT";
                        return;
                    }
                }
            }
            else
            {
                if (m_Altes_Binding.Bezeichnung != shortcutText.ToString())
                {

                    foreach (M_Binding item in m_Auflistung_Keybinds)
                    {
                        if (item.Bezeichnung == shortcutText.ToString())
                        {
                            Hotkey_Edit = "BELEGT";
                            return;
                        }
                    }
                }
            }





            // Update Textbox via Binding
            if (!isEditMode)
            {
                Hotkey = shortcutText.ToString();
            }
            else
            {
                Hotkey_Edit = shortcutText.ToString();
            }
            


            // Den Hotkey speichern
            CurrentKeyValue = (int)key;
        }
        private bool CanExecute_PreviewKeyDown(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Edit
        private DelegateCommand m_Command_Button_Edit;
        public DelegateCommand Command_Button_Edit
        {
            get
            {
                if (m_Command_Button_Edit == null)
                { m_Command_Button_Edit = new DelegateCommand(Execute_Command_Button_Edit, CanExecute_Command_Button_Edit); }

                return m_Command_Button_Edit;
            }
        }
        private void Execute_Command_Button_Edit(object Parameter)
        {
            if ((M_Binding)Parameter == null)
            {
                return;
            }

            m_Altes_Binding = (M_Binding)Parameter;

            Hotkey_Edit = m_Altes_Binding.Bezeichnung;
            AutoEnter_Edit = m_Altes_Binding.AutoEnter;
            for (int i = 0; i < m_Altes_Binding.Auflistung_BindingOptions.Count; i++)
            {
                Command_Edit += m_Altes_Binding.Auflistung_BindingOptions[i].cmd;

                if (i+1 != m_Altes_Binding.Auflistung_BindingOptions.Count)
                {
                    Command_Edit += "~";
                }
            }

            CurrentKeyValue = m_Altes_Binding.KeyValue;
            CurrentKeyModValue = m_Altes_Binding.ModValue;

            isEditMode = true;

            _CONTROLLER.SetzeUC(_CONTROLLER.VCustomKeybindsEdit, _CONTROLLER.VMCustomKeybinds);

        }
        private bool CanExecute_Command_Button_Edit(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Edit_Save
        private DelegateCommand m_Command_Button_Edit_Save;
        public DelegateCommand Command_Button_Edit_Save
        {
            get
            {
                if (m_Command_Button_Edit_Save == null)
                { m_Command_Button_Edit_Save = new DelegateCommand(Execute_Command_Button_Edit_Save, CanExecute_Command_Button_Edit_Save); }

                return m_Command_Button_Edit_Save;
            }
        }
        private void Execute_Command_Button_Edit_Save(object Parameter)
        {
            // Suche das alte Binding in der Liste
            for (int i = 0; i < m_Auflistung_Keybinds.Count; i++)
            {
                if (m_Altes_Binding.Bezeichnung == m_Auflistung_Keybinds[i].Bezeichnung & m_Altes_Binding.KeyValue == m_Auflistung_Keybinds[i].KeyValue && m_Altes_Binding.ModValue == m_Auflistung_Keybinds[i].ModValue && m_Altes_Binding.Auflistung_BindingOptions[0].cmd == m_Auflistung_Keybinds[i].Auflistung_BindingOptions[0].cmd)
                {
                    m_Auflistung_Keybinds[i].AutoEnter = AutoEnter_Edit;
                    m_Auflistung_Keybinds[i].Bezeichnung = Hotkey_Edit;
                    m_Auflistung_Keybinds[i].KeyValue = CurrentKeyValue;
                    m_Auflistung_Keybinds[i].KeyValueForms = (int)(System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey((Key)CurrentKeyValue);
                    m_Auflistung_Keybinds[i].ModValue = CurrentKeyModValue;

                    //Reset Commands
                    m_Auflistung_Keybinds[i].Auflistung_BindingOptions = new List<M_BindingOption>();

                    // Command aufsplitten, wenn Contains ~
                    if (m_Command_Edit.Contains("~"))
                    {
                        if (!AutoEnter_Edit)
                        {
                            // Hinweis Anzeigen, wenn Mehrere Commands da sind, aber AutoEnter False ist
                            if (MessageBoxResult.No == Bindermessage.ShowQuestionYesNo("Sie haben den Hotkey mit mehreren Befehlen belegt und die Auto-Enter Funktion deaktiviert!\nBeachten Sie, dass hierdurch nur der letzte Befehl nicht automatisch abgesendet wird!\n\nMöchten Sie den Hotkey trotzdem hinzufügen?"))
                            {
                                return;
                            }
                        }

                        string[] commands = m_Command_Edit.Split('~');
                        foreach (string command in commands)
                        {
                            M_BindingOption opt = new M_BindingOption();
                            opt.cmd = command.TrimStart();
                            m_Auflistung_Keybinds[i].Auflistung_BindingOptions.Add(opt);
                        }
                    }
                    else
                    {
                        M_BindingOption opt = new M_BindingOption();
                        opt.cmd = m_Command_Edit.TrimStart();
                        m_Auflistung_Keybinds[i].Auflistung_BindingOptions.Add(opt);
                    }
                }
            }

            Hotkey_Edit = String.Empty;
            Command_Edit = String.Empty;
            isEditMode = false;

            OnPropertyChanged("Auflistung_Keybinds");

            Auflistung_Keybinds = new ObservableCollection<M_Binding>(m_Auflistung_Keybinds);

            _CONTROLLER.SetzeUC(_CONTROLLER.VCustomKeybinds, _CONTROLLER.VMCustomKeybinds);


        }
        private bool CanExecute_Command_Button_Edit_Save(object Parameter)
        {
            return true;
        }
        #endregion

        #region DelegateCommand Command_Button_Edit_Cancel
        private DelegateCommand m_Command_Button_Edit_Cancel;
        public DelegateCommand Command_Button_Edit_Cancel
        {
            get
            {
                if (m_Command_Button_Edit_Cancel == null)
                { m_Command_Button_Edit_Cancel = new DelegateCommand(Execute_Command_Button_Edit_Cancel, CanExecute_Command_Button_Edit_Cancel); }

                return m_Command_Button_Edit_Cancel;
            }
        }
        private void Execute_Command_Button_Edit_Cancel(object Parameter)
        {
            Hotkey_Edit = String.Empty;
            Command_Edit = String.Empty;
            isEditMode = false;
            _CONTROLLER.SetzeUC(_CONTROLLER.VCustomKeybinds, _CONTROLLER.VMCustomKeybinds);
        }
        private bool CanExecute_Command_Button_Edit_Cancel(object Parameter)
        {
            return true;
        }
        #endregion

        #endregion
    }
}
