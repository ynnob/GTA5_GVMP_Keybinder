using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTA5_Keybinder_Bonnyfication.Models
{
    public class M_Settings
    {
        private int m_Delay;
        public int Delay
        {
            get { return m_Delay; }
            set { m_Delay = value; }
        }

        private int m_MultiCommanDelay;
        public int MultiCommanDelay
        {
            get { return m_MultiCommanDelay; }
            set { m_MultiCommanDelay = value; }
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
    }
}
