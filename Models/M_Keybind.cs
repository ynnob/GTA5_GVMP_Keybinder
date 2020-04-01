using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA5_Keybinder_Bonnyfication.Models
{
    /// <summary>
    /// Old Binding Class
    /// Just in Case i need to convert to the new version
    /// </summary>
    public class M_Keybind
    {
        private string m_cmd;
        public string cmd
        {
            get { return m_cmd; }
            set { m_cmd = value; }
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
