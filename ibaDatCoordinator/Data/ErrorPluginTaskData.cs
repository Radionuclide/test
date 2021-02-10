using iba.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Data
{
    [Serializable]
    class ErrorPluginTaskData : IPluginTaskData, IPluginTaskDataIsSame
    {
        public ErrorPluginTaskData()
        {

        }

        public string ErrorMessage;

        public string PluginType;
        public string PluginName;
        public string SavedConfig;

        public string NameInfo => PluginName;

        public int DongleBitPos => -1;

        public IPluginControl GetControl()
        {
            return new Controls.ErrorPluginTaskControl();
        }

        public IPluginTaskWorker GetWorker()
        {
            return new Processing.ErrorPluginTaskWorker(ErrorMessage);
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
        }

        public void Reset(IDatCoHost host)
        {
        }

        public void SetParentJob(IJobData data)
        {
        }

        public object Clone()
        {
            return new ErrorPluginTaskData()
            {
                ErrorMessage = ErrorMessage,
                PluginType = PluginType,
                PluginName = PluginName,
                SavedConfig = SavedConfig
            };
        }

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            ErrorPluginTaskData other = data as ErrorPluginTaskData;
            if (other == null)
                return false;

            return PluginType == other.PluginType &&
                PluginName == other.PluginName &&
                SavedConfig == other.SavedConfig;
        }
    }
}
