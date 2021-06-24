using System;
using iba.Data;
using iba.Processing;

namespace iba.Utility
{
    internal static class SetNextNameHelper
    {
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
            else return name.Substring(0, name.LastIndexOf('_'));
        }

        private static string CalcNextName(TaskData taskData, ConfigurationData confData = null)
        {
            string name = (confData == null) ? taskData.Name : confData.Name;
            int index = -1;
            bool found = false;
            string root = GetRoot(name);
            foreach (ConfigurationData cData in TaskManager.Manager.Configurations)
            {
                if (confData != null)
                {
                    if (root.Equals(GetRoot(cData.Name)))
                    {
                        index = Math.Max(index, GetIndex(cData.Name));
                        if (name.Equals(cData.Name))
                            found = true;
                    }
                }
                else
                {
                    foreach (TaskData tData in cData.Tasks)
                    {
                        if (root.Equals(GetRoot(tData.Name)))
                        {
                            index = Math.Max(index, GetIndex(tData.Name));
                            if (name.Equals(tData.Name))
                                found = true;
                        }
                    }
                }
            }
            return !found
                ? name //original name
                : $"{root}_{index + 1}"; // name with an incremented index
        }

        public static void SetNextName(this TaskData taskData) => taskData.Name = CalcNextName(taskData);

        public static void SetNextName(this ConfigurationData taskData) => taskData.Name = CalcNextName(null, taskData);
    }
}
