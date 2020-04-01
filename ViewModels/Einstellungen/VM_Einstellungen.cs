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

namespace GTA5_Keybinder_Bonnyfication.ViewModels.Einstellungen
{
    public class VM_Einstellungen : Base.ViewModelBase
    {
        #region Konstruktor
        public VM_Einstellungen(MainController controller)
        {
            // CONTROLLER zuweisen
            if (controller == null)
            {
                Bindermessage.ShowError("Die Initialisierung ist fehlgeschlagen! Das Programm wird beendet.");
                return;
            }

            _CONTROLLER = controller;

            // Delay aus Settings setzen
            Delay = _CONTROLLER.Aktuelle_Einstellungen.Delay.ToString();
            MultiCommanDelay = _CONTROLLER.Aktuelle_Einstellungen.MultiCommanDelay.ToString();
            Name = _CONTROLLER.Aktuelle_Einstellungen.Name;
        }
        #endregion

        #region Properties
        private MainController _CONTROLLER;

        private string m_Delay;
        public string Delay
        {
            get { return m_Delay; }
            set { m_Delay = value; OnPropertyChanged("Delay"); }
        }

        private string m_MultiCommanDelay;
        public string MultiCommanDelay
        {
            get { return m_MultiCommanDelay; }
            set { m_MultiCommanDelay = value; OnPropertyChanged("MultiCommanDelay"); }
        }


        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; OnPropertyChanged("Name"); }
        }



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
        private string m_Hotkey = String.Empty;
        public string Hotkey
        {
            get { return m_Hotkey; }
            set { m_Hotkey = value; OnPropertyChanged("Hotkey"); }
        }
        #endregion

        #region Commands
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
            M_Settings set = new M_Settings();

            int i = 1;

            if (Int32.TryParse(Delay,out i))
            {
                set.Delay = Convert.ToInt32(Delay);
            }
            else
            {
                Bindermessage.ShowWarning("Bitte geben Sie das Delay in Millisekunden an!\nEs sind nur positive Ganzzahlen erlaubt.");
                return;
            }

            if (Int32.TryParse(MultiCommanDelay, out i))
            {
                set.MultiCommanDelay = Convert.ToInt32(MultiCommanDelay);
            }
            else
            {
                Bindermessage.ShowWarning("Bitte geben Sie das MultiCommandDelay in Millisekunden an!\nEs sind nur positive Ganzzahlen erlaubt.");
                return;
            }

            set.Name = Name;

            string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string m_foldername = "Bonnyfication";
            string m_FileName = "Keybinds_Settings.xml";
            string m_Fullpath = Path.Combine(m_appdatapath, m_foldername, m_FileName);

            try
            {
                XmlSerializer seri = new XmlSerializer(typeof(M_Settings));
                using (StreamWriter writ = new StreamWriter(m_Fullpath))
                {
                    seri.Serialize(writ, set);
                }
            }
            catch (Exception ex)
            {
                Bindermessage.ShowError("Fehler beim Speichern der Einstellungen!\n\nError:\n" + ex);
            }


            _CONTROLLER.RestartKeybinder();
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
            Delay = _CONTROLLER.Aktuelle_Einstellungen.Delay.ToString();
            Name = _CONTROLLER.Aktuelle_Einstellungen.Name;
            _CONTROLLER.SetzeUC(_CONTROLLER.VMainWindow, _CONTROLLER.VMMainWindow);
        }
        private bool CanExecute_Command_Button_Cancel(object Parameter)
        {
            return true;
        }

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
                Hotkey = "ERROR";
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


            foreach (M_Binding item in _CONTROLLER.Auflistung_Keybinds)
            {
                if (item.Bezeichnung == shortcutText.ToString())
                {
                    Hotkey = "BELEGT";
                    return;
                }
            }
            
            Hotkey = shortcutText.ToString();


            // Den Hotkey abspeichern
            CurrentKeyValue = (int)key;
        }
        private bool CanExecute_PreviewKeyDown(object Parameter)
        {
            return true;
        }
        #endregion
        #endregion
    }
}
