using System;

namespace iba.Plugins
{
    /// <summary>
    /// give an instance of a class derived from this class to return worker status
    /// </summary>
    [Serializable]
    public class PluginTaskWorkerStatus
    {
        public bool started; //wheter or not the plugin is started
        public object extraData; //any extra data you want the plugin control to know
    }
}
