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

public class KeybindToAHK
{
    [Flags]
    public enum ModifierKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
    }

    public string GetKeyString(M_Binding keybind)
    {
        // INFO
        // Gibt den AHK Hotkey +   ::\r\n    zurück
        /*
         *  PREFIXE
         *  Shift = +
         *  Alt = !
         *  Control = ^
         * 
         * 
         * 
         * */
        string m_AHK = String.Empty;

        int m_Value = keybind.KeyValue;
        ModifierKeys mod = (ModifierKeys)keybind.ModValue;

        switch (mod)
        {
            case ModifierKeys.Alt:

                m_AHK += "!";

                foreach (AHKKey key in Enum.GetValues(typeof(AHKKey)))
                {
                    // Suche gültigen AHK Key
                    if ((int)key == m_Value)
                    {
                        if (key.ToString().Contains("Decimal"))
                        {
                            m_AHK += key.ToString().Remove(0,7);
                        }
                        else
                        {
                            m_AHK += key.ToString();
                        }

                        break;
                    }
                }

                break;
            case ModifierKeys.Control:
                m_AHK += "^";

                foreach (AHKKey key in Enum.GetValues(typeof(AHKKey)))
                {
                    // Suche gültigen AHK Key
                    if ((int)key == m_Value)
                    {
                        if (key.ToString().Contains("Decimal"))
                        {
                            m_AHK += key.ToString().Remove(0, 7);
                        }
                        else
                        {
                            m_AHK += key.ToString();
                        }

                        break;
                    }
                }
                break;
            case ModifierKeys.Shift:
                m_AHK += "+";

                foreach (AHKKey key in Enum.GetValues(typeof(AHKKey)))
                {
                    // Suche gültigen AHK Key
                    if ((int)key == m_Value)
                    {
                        if (key.ToString().Contains("Decimal"))
                        {
                            m_AHK += key.ToString().Remove(0, 7);
                        }
                        else
                        {
                            m_AHK += key.ToString();
                        }

                        break;
                    }
                }

                break;
            case ModifierKeys.None:

                foreach (AHKKey key in Enum.GetValues(typeof(AHKKey)))
                {
                    // Suche gültigen AHK Key
                    if ((int)key == m_Value)
                    {
                        if (key.ToString().Contains("Decimal"))
                        {
                            m_AHK += key.ToString().Remove(0, 7);
                        }
                        else
                        {
                            m_AHK += key.ToString();
                        }

                        break;
                    }
                }

                break;
        }

        // Add :: to the string
        m_AHK += "::\r\n";


        return m_AHK;
    }

    public void CreateAHKFile(ObservableCollection<M_Binding> ListeKeybinds, M_Settings m_Einstellungen)
    {
        // Directory
        string m_appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string m_foldername = "Bonnyfication";
        string m_FileName = "Keybinds_Script.ahk";
        string m_Fullpath = Path.Combine(m_appdatapath, m_foldername, m_FileName);

        // Lade Default String für die AHK Datei mit Standardfunktionen
        string m_defaultString = GetDefaultSettingsAHK(m_Einstellungen);
        string m_CloseTag = "return\r\n";

        List<string> m_AuflistungHotkeys = new List<string>();

        foreach (M_Binding k in ListeKeybinds)
        {
            string m_NewKeybind = String.Empty;
            m_NewKeybind += "\r\n";

            // Hole den Befehl im AHK Format
            if (GetKeyString(k) == String.Empty)
            {
                Bindermessage.ShowWarning("Ein Hotkey konnte nicht konvertiert werden!\n\nFehlerhafter Hotkey:\n" + k.Bezeichnung);
                continue;
            }
            
            m_NewKeybind += GetKeyString(k);

            // Für jedes Command in der Commands Auflistung

            if (k.Auflistung_BindingOptions.Count > 1) // MEHRERE COMMANDS 
            {
                for (int i = 0; i < k.Auflistung_BindingOptions.Count(); i++)
                {
                    if (i+1 == k.Auflistung_BindingOptions.Count())
                    {
                        if (k.AutoEnter)
                        {
                            m_NewKeybind += "SendMessage(\"" + GetEscapedString(k.Auflistung_BindingOptions[i].cmd) + "\")\r\n";
                        }
                        else
                        {
                            m_NewKeybind += "SendMessageNoEnter(\"" + GetEscapedString(k.Auflistung_BindingOptions[i].cmd) + " \")\r\n";
                        }
                    }
                    else
                    {
                        m_NewKeybind += "SendMessage(\"" + GetEscapedString(k.Auflistung_BindingOptions[i].cmd) + "\")\r\n";
                        m_NewKeybind += "Sleep, " + m_Einstellungen.MultiCommanDelay.ToString() + "\r\n";
                    }
                }
            }
            else // Ein Command
            {
                if (k.AutoEnter)
                {
                    m_NewKeybind += "SendMessage(\"" + GetEscapedString(k.Auflistung_BindingOptions[0].cmd) + "\")\r\n";
                }
                else
                {
                    m_NewKeybind += "SendMessageNoEnter(\"" + GetEscapedString(k.Auflistung_BindingOptions[0].cmd) + " \")\r\n";
                }
            }


            m_NewKeybind += m_CloseTag;
            m_AuflistungHotkeys.Add(m_NewKeybind);
        }

        //Create the AHK File
        using (StreamWriter wr = new StreamWriter(m_Fullpath))
        {
            //Write Defaults
            wr.WriteLine(m_defaultString);

            foreach (string s in m_AuflistungHotkeys)
            {
                wr.WriteLine(s);
            }
        }
    }

    private string GetEscapedString(string cmd)
    {
        // Check string with "
        if (cmd.Contains("\""))
        {
            return cmd.Replace("\"", "\"\"");
        }
        else
        {
            return cmd;
        }
    }

    private string GetDefaultSettingsAHK(M_Settings m_Einstellungen)
    {
        string m_DefaultString = "; Bonnyfication GTAV Roleplay Keybinder\r\n" +
            "#IfWinActive, Grand Theft Auto V\r\n" +
            "#SingleInstance, Force\r\n" +
            "#Persistent\r\n" +
            "#UseHook, On\r\n" +
            "SetKeyDelay, 0, " + m_Einstellungen.Delay.ToString() + "\r\n" +
            "t::\r\n" +
            "Suspend On\r\n" +
            "SendInput t\r\n" +
            "Hotkey, Enter, On\r\n" +
            "Hotkey, Escape, On\r\n" +
            "Hotkey, t, Off\r\n" +
            "return\r\n" +
            "NumPadEnter::\r\n" +
            "Enter::              \r\n" +
            "Suspend Permit\r\n" +
            "Suspend Off                                \r\n" +
            "SendInput {Enter}\r\n" +
            "Hotkey, t, On\r\n" +
            "Hotkey, Enter, Off\r\n" +
            "Hotkey, Escape, Off\r\n" +
            "return\r\n" +
            "Escape::\r\n" +
            "Suspend Permit\r\n" +
            "Suspend Off\r\n" +
            "SendInput {Escape}\r\n" +
            "Hotkey, t, On\r\n" +
            "Hotkey, Enter, Off\r\n" +
            "Hotkey, Escape, Off\r\n" +
            "return\r\n" +
            "SendMessage(message)\r\n" +
            "{\r\n" +
            "SetKeyDelay, 0, 30\r\n" +
            "    clipboardBuffer := ClipboardAll\r\n" +
            "    Clipboard := message\r\n" +
            "    SendEvent, t^v{ENTER}\r\n" +
            "    Clipboard := clipboardBuffer\r\n" +
            "    return\r\n" +
            "}\r\n" +
            "SendMessageNoEnter(message)\r\n" +
            "{\r\n" +
            "SetKeyDelay, 0, 30\r\n" +
            "    clipboardBuffer := ClipboardAll\r\n" +
            "    Clipboard := message\r\n" +
            "    SendEvent, t^v\r\n" +
            "    Clipboard := clipboardBuffer\r\n" +
            "    return\r\n" +
            "}\r\n" +
            "Pause::		\r\n" +
            "Suspend\r\n" +
            "	SendMessage(\"/ooc Der GVMP-Keybinder by Bonnyfication wurde Aktiviert oder Deaktiviert!\")\r\n" +
            "return\r\n";

        return m_DefaultString;
    }

}



/*
 * DEFAULT AHK
 * 
 * 
 * 
 * ; Bonnyfication GTAV Roleplay Keybinder
#IfWinActive, Grand Theft Auto V
#SingleInstance, Force
#Persistent
#UseHook, On
SetKeyDelay, 0, 55
t::
Suspend On
SendInput t
Hotkey, Enter, On
Hotkey, Escape, On
Hotkey, t, Off
return
NumPadEnter::
Enter::              
Suspend Permit
Suspend Off                                
SendInput {Enter}
Hotkey, t, On
Hotkey, Enter, Off
Hotkey, Escape, Off
return
Escape::
Suspend Permit
Suspend Off
SendInput {Escape}
Hotkey, t, On
Hotkey, Enter, Off
Hotkey, Escape, Off
return

SendMessage(message)
{
    clipboardBuffer := ClipboardAll
    Clipboard := message
    SendEvent, t^v{ENTER}
    Clipboard := clipboardBuffer
    return
}
SendMessageNoEnter(message)
{
    clipboardBuffer := ClipboardAll
    Clipboard := message
    SendEvent, t^v
    Clipboard := clipboardBuffer
    return
}

Pause::		
Suspend
	SendMessage("/me Bonnyfication Keybinder On/Off")
return

 * */
