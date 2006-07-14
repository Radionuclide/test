using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Data
{
    public class WatchDogData
    {
        private bool m_enabled;
        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }
        
        private int m_cycleTime; //in s
        public int CycleTime
        {
            get { return m_cycleTime; }
            set { m_cycleTime = value; }
        }

        private int m_portNr;
        public int PortNr
        {
            get { return m_portNr; }
            set { m_portNr = value; }
        }

        private string m_address;
        public string Address
        {
            get { return m_address; }
            set { m_address = value; }
        }

        private bool m_activeNode;
        public bool ActiveNode
        {
            get { return m_activeNode; }
            set { m_activeNode = value; }
        }

        public WatchDogData()
        { 
            //default settings
            m_enabled = false;
            m_cycleTime = 10;
            m_portNr = 40002;
            m_address = "";
            m_activeNode = false;
        }

        public object Clone()
        {
            WatchDogData wd = new WatchDogData();
            wd.m_activeNode = m_activeNode;
            wd.m_address = m_address;
            wd.m_cycleTime = m_cycleTime;
            wd.m_enabled = m_enabled;
            wd.m_portNr = m_portNr;
            return wd;
        }

        public override bool Equals(object obj)
        {
            WatchDogData temp = obj as WatchDogData;
            if (temp == null) return false;
            return temp.m_activeNode == m_activeNode
                && temp.m_address == m_address
                && temp.m_cycleTime == m_cycleTime
                && temp.m_enabled == m_enabled
                && temp.m_portNr == m_portNr;
        }

        public override int GetHashCode()
        {
            return m_activeNode.GetHashCode()
                ^ m_address.GetHashCode()
                ^ m_cycleTime.GetHashCode()
                ^ m_enabled.GetHashCode()
                ^ m_portNr.GetHashCode();
        }
    }
}
