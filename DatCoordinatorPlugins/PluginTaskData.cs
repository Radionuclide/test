using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Plugins
{

    /// <summary>
    /// Interface implemented by a plugin task, data part
    /// Implementation must have the Serializable attribute
    /// </summary>
    public interface IPluginTaskData : ICloneable
    {
        /// <summary>
        /// Get GUI Control 
        /// If the resulting class is not a Control, 
        /// this will result in error.v
        /// </summary>
        IPluginControl GetControl();

        /// <summary>
        /// Function to get the worker part of the plugin
        /// </summary>
        IPluginTaskWorker GetWorker();

        /// <summary>
        /// Function to get information about the plugin
        /// </summary>
        PluginTaskInfo GetInfo();
        
        ///
        IJobData ParentJob {get; set;}
    }

    /// <summary>
    /// Interface implemented by a plugin task, data part in the case that the plugin needs
    /// to use the datcoordinator network shares handling functionallity
    /// Implementation must have the Serializable attribute
    /// </summary>
    public interface IPluginTaskDataUNC : IPluginTaskData
    {
        /// <summary>
        /// Function to be called when any Paths need their corresponding UNC path updated
        /// If the plugin has paths that need updating to UNC paths, implement this function,
        /// otherwise, have an empty implementation.
        /// </summary>
        void UpdateUNC();

        /// <summary>
        /// If any paths need to be registered with the SharesHandler of the datcoordinator, return them here
        /// {{path1,username1, pass1},{path2,username2, pass2},...}
        /// otherwise return null
        /// </summary>
        /// <returns>UNC paths</returns>
        string[][] UNCPaths();
    }
}
