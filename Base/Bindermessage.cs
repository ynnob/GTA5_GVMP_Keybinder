using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public static class Bindermessage
{
    private static string m_Titel = "GVMP Keybinder";

    public static void ShowInfo(string message)
    {
        MessageBox.Show(message, m_Titel,MessageBoxButton.OK,MessageBoxImage.Information);
    }

    public static void ShowWarning(string message)
    {
        MessageBox.Show(message, m_Titel, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public static void ShowError(string message)
    {
        MessageBox.Show(message, m_Titel, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public static MessageBoxResult ShowQuestionYesNo(string message)
    {
        return MessageBox.Show(message, m_Titel, MessageBoxButton.YesNo, MessageBoxImage.Question);
    }
}

