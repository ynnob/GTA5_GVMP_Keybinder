using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA5_Keybinder_Bonnyfication.Models
{
    /// <summary>
    /// New Binding Class
    /// </summary>
    public class M_Binding
    {
        private List<M_BindingOption> m_Auflistung_BindingOptions;
        public List<M_BindingOption> Auflistung_BindingOptions
        {
            get { return m_Auflistung_BindingOptions; }
            set { m_Auflistung_BindingOptions = value; }
        }

        private bool m_AutoEnter;
        public bool AutoEnter
        {
            get { return m_AutoEnter; }
            set { m_AutoEnter = value; }
        }


        private int m_KeyValue;
        public int KeyValue
        {
            get { return m_KeyValue; }
            set { m_KeyValue = value; }
        }

        private int m_KeyValueForms;
        public int KeyValueForms
        {
            get { return m_KeyValueForms; }
            set { m_KeyValueForms = value; }
        }

        private int m_ModValue;
        public int ModValue
        {
            get { return m_ModValue; }
            set { m_ModValue = value; }
        }

        private string m_Bezeichnung;
        public string Bezeichnung
        {
            get { return m_Bezeichnung; }
            set { m_Bezeichnung = value; }
        }
    }
}
