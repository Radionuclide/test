using System;
using iba.Data;
using iba.Processing;

namespace iba.Utility
{
    class SetNextName
    {
        private TaskData m_tdata;
        private ConfigurationData m_cdata;
        private string m_name;

        public SetNextName(TaskData data)
        {
            m_tdata = data;
            m_cdata = null;
            CalcNextName();
            data.Name = m_name;
        }

        public SetNextName(ConfigurationData data)
        {
            m_cdata = data;
            m_tdata = null;
            CalcNextName();
            data.Name = m_name;
        }

        private static int GetIndex(string name)
        {
            int pos = name.LastIndexOf('_');
            if (pos < 0) return 0;
            string numberPart = name.Substring(pos + 1);
            if (int.TryParse(numberPart, out pos))
                return pos;
            else
                return 0;
        }

        private static string GetRoot(string name)
        {
            if (GetIndex(name) == 0) return name;
            else return name.Substring(0,name.LastIndexOf('_'));
        }

        private void CalcNextName()
        {
            m_name = CalcNextName(m_tdata, m_cdata);
        }

        
        public static string CalcNextName(TaskData taskData, ConfigurationData confData = null)
        {
            string name = (confData == null) ? taskData.Name : confData.Name;
            int index = -1;
            bool found = false;
            string root = GetRoot(name);
            foreach (ConfigurationData cdata in TaskManager.Manager.Configurations)
            {
                if (confData != null)
                {
                    if (root.Equals(GetRoot(cdata.Name)))
                    {
                        index = Math.Max(index, GetIndex(cdata.Name));
                        if (name.Equals(cdata.Name))
                            found = true;
                    }
                }
                else foreach (TaskData tdata in cdata.Tasks)
                {
                    if (root.Equals(GetRoot(tdata.Name)))
                    {
                        index = Math.Max(index, GetIndex(tdata.Name));
                        if (name.Equals(tdata.Name))
                            found = true;
                    }
                }
            }
            return !found 
                ? name //original name
                : $"{root}_{index + 1}"; // name with an incremented index
        }
    }
}
